using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine.UI;
using Microsoft.SqlServer.Server;

public class DialogueEditor : EditorWindow
{
    public string currentAssetName;
    public Dialogue currentAsset;
    [SerializeField] public UnityEvent<Dialogue> OnDialogueStart;
    [SerializeField] public UnityEvent<Dialogue> OnDialogueFinish;


    public DialoguePage currentPage;

    int pageNum;

    bool dialogueFoldout = false;
    bool pageFoldout = false;
    bool characterFoldout = true;

    string nextButtonText = "Next";

    Vector2 scrollPosition;

    [MenuItem("Window/DialogueEditor")]
    public static void ShowWindow()
    {
        GetWindow<DialogueEditor>();
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        currentAsset = EditorGUILayout.ObjectField("Dialogue to Load", currentAsset, typeof(Dialogue), false) as Dialogue;
        if (GUILayout.Button("Load"))
        {
            if (currentAsset != null)
            {
                ImportDialogueAsset(currentAsset.name);
                LoadPage(pageNum);
            }
            
        }
        if (currentAsset == null)
        {
            EditorGUILayout.EndScrollView();
            return;
        }
    
        GUILayout.Space(10) ;

        dialogueFoldout = EditorGUILayout.Foldout(dialogueFoldout, "Dialogue Events", true);
        if (dialogueFoldout)
        {
            GUILayout.BeginHorizontal();
            SerializedObject s = new SerializedObject(currentAsset);
            EditorGUILayout.PropertyField((s.FindProperty("OnDialogueStart")));
            EditorGUILayout.PropertyField(s.FindProperty("OnDialogueFinish"));
            s.ApplyModifiedProperties();
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(10);

        if (currentAsset.pages.Count == 0)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("This dialogue has no pages.");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add a Page", GUILayout.ExpandWidth(false)))
            {
                AddPage();
                GUI.FocusControl(null);
            };
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
        }

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Previous", GUILayout.ExpandWidth(false))){
            if (pageNum > 0)
            {
                LoadPage(pageNum - 1);
            }
            GUI.FocusControl(null);
        };
        EditorGUIUtility.labelWidth = 40;
        GUILayout.Label(string.Format("Page {0}/{1}", pageNum+1, currentAsset.pages.Count));
        EditorGUIUtility.labelWidth = 0;
        if (GUILayout.Button(nextButtonText, GUILayout.ExpandWidth(false)))
        {
            if (pageNum < currentAsset.pages.Count-1)
            {
                LoadPage(pageNum + 1);
            }
            else if (pageNum == currentAsset.pages.Count-1)
            {
                AddPage();
            }
            GUI.FocusControl(null);
        };
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Delete", GUILayout.ExpandWidth(false)))
        {
            if (EditorUtility.DisplayDialog("Delete Page", "Would you like to delete this page of Dialogue?", "Yes", "No"))
            {
                DeletePage(pageNum);
                LoadPage(pageNum < currentAsset.pages.Count - 1 ? pageNum + 1 : pageNum - 1);
            }          
            GUI.FocusControl(null);
        };
        GUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = 60;
        currentPage.FontSize = EditorGUILayout.IntField("Font Size", currentPage.FontSize,GUILayout.ExpandWidth(false));
        currentPage.content = EditorGUILayout.TextField("Content",currentPage.content,GUILayout.MinHeight(50));
        EditorGUIUtility.labelWidth = 0;

        GUILayout.Space(10);

        characterFoldout = EditorGUILayout.Foldout(characterFoldout, "Participants", true);
        if (characterFoldout)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUIUtility.fieldWidth = 120;
            for (int i = 0; i< currentPage.characters.Count; i++)  //each card
            {
                string participantName = "";

                DialogueCharacterContext ctx = currentPage.characters[i];
                //card heading
                GUILayout.BeginVertical("textarea");
                GUILayout.Label(string.Format("Participant {0} ({1})", (i+1), (ctx.character != null ? ctx.character.name : "Not Selected")) ,EditorStyles.boldLabel );
                
                //character asset
                ctx.character = EditorGUILayout.ObjectField("Character Asset", ctx.character, typeof(DialogueCharacter), false) as DialogueCharacter;

                if (ctx.character != null) {
                    int spriteIndex = ctx.character.GetSpriteIndex(ctx.currentSpriteName);
                    spriteIndex = EditorGUILayout.Popup("Sprite", spriteIndex, ctx.character.GetSpriteNames());
                    ctx.currentSpriteName = ctx.character.GetSpriteNameAt(spriteIndex);

                    Texture2D preview = ctx.character.GetSpriteAt(spriteIndex).texture;
                    participantName = ctx.character.name;
                }
                
                //position slider
                ctx.position = EditorGUILayout.Slider("Position",ctx.position, 0, 1);

                //isActive Toggle
                GUILayout.BeginHorizontal();
                GUILayout.Label("Is Active?");
                GUIStyle isActiveButton = new GUIStyle("button");
                isActiveButton.normal.textColor = ctx.isActive ? Color.green : Color.red;
                isActiveButton.hover.textColor = ctx.isActive ? Color.green : Color.red;
                if (GUILayout.Button(ctx.isActive.ToString(), isActiveButton))
                {
                    ctx.isActive = !ctx.isActive;
                }
                GUILayout.EndHorizontal();

                //FacingDirection Toggle
                GUILayout.BeginHorizontal();
                GUILayout.Label("Facing Direction");
                if (GUILayout.Button(ctx.isFlipped ? "<< Left " : "Right >>"))
                {
                    ctx.isFlipped = !ctx.isFlipped;
                }
                GUILayout.EndHorizontal();


                //Move/Delete Group
                GUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(i == 0);
                if (GUILayout.Button("<<"))
                {
                    currentPage.characters.Insert(i - 1, currentPage.characters[i]);
                    currentPage.characters.RemoveAt(i + 1);
                }
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Remove"))
                {
                    if (EditorUtility.DisplayDialog("Remove Participant", "Would you like to remove this Participant from this Page?", "Yes", "No"))
                    {
                        currentPage.characters.RemoveAt(i);
                        i--;
                    }
                }
                EditorGUI.BeginDisabledGroup(i == currentPage.characters.Count - 1);
                if (GUILayout.Button(">>"))
                {
                    currentPage.characters.Insert(i + 2, currentPage.characters[i]);
                    currentPage.characters.RemoveAt(i);
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();


                GUILayout.EndVertical();

            }
            EditorGUIUtility.fieldWidth = 0;


            if (GUILayout.Button("Add", GUILayout.ExpandWidth(false)))
            {
                currentPage.characters.Add(new DialogueCharacterContext());
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            
            
        }


        GUILayout.Space(10);
        pageFoldout = EditorGUILayout.Foldout(pageFoldout, "Page Events", true);
        if (pageFoldout)
        {
            GUILayout.BeginHorizontal();
            SerializedObject s = new SerializedObject(currentAsset);
            SerializedProperty p = s.FindProperty("pages").GetArrayElementAtIndex(pageNum);
            EditorGUILayout.PropertyField(p.FindPropertyRelative("OnPageStart"));
            EditorGUILayout.PropertyField(p.FindPropertyRelative("OnPageFinish"));
            s.ApplyModifiedProperties();
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }


    void ImportDialogueAsset(string name)
    {
        currentAssetName = currentAsset.name;
        OnDialogueStart = currentAsset.OnDialogueStart;
        OnDialogueFinish = currentAsset.OnDialogueFinish;
        pageNum = 1;
    }

    void LoadPage(int i)
    {
        if (i > currentAsset.pages.Count-1 || currentAsset.pages.Count == 0)
            return;

        currentPage = currentAsset.pages[i];
        pageNum = i;

        nextButtonText = (pageNum == currentAsset.pages.Count - 1) ? "Add" : "Next";

        
    }

    void AddPage()
    {
        currentAsset.pages.Add(new DialoguePage());
        LoadPage(currentAsset.pages.Count-1);
    }

    void DeletePage(int i)
    {
        currentAsset.pages.RemoveAt(i);
    }
}
