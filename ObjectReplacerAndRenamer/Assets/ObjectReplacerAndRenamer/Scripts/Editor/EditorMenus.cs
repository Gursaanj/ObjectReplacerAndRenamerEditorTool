﻿using UnityEditor;

namespace GursaanjTools
{
    public static class EditorMenus
    {
        #region Scene Object EditorWindows

        #region Object Replacer Tool
        
        [MenuItem("GursaanjTools/Level Tools/Replace Selected Objects")]
        public static void ReplaceObjectsTool()
        {
            ReplaceObjects_Editor.Init(typeof(ReplaceObjects_Editor), EditorWindowData.EditorWindowInformations["Object Replacer"]);
        }

        #endregion

        #region Object Renamer

        [MenuItem("GursaanjTools/Level Tools/Rename Selected Objects")]
        public static void RenameObjectsTool()
        {
            RenameObjects_Editor.Init(typeof(RenameObjects_Editor), EditorWindowData.EditorWindowInformations["Object Renamer"]);
        }

        #endregion

        #region Object Grouper

        [MenuItem("GursaanjTools/Level Tools/Group Selected Objects %#z")]
        public static void GroupObjectsTool()
        {
            GroupObjects_Editor.Init(typeof(GroupObjects_Editor), EditorWindowData.EditorWindowInformations["Object Grouper"]);
        }

        #endregion

        #region Object Ungrouper
        
        [MenuItem("GursaanjTools/Level Tools/Ungroup Selected Objects %#q")]
        public static void UngroupObjectsTool()
        {
            UngroupObjects_Editor.Init();
        }

        #endregion

        #region Object Aligner

        [MenuItem("GursaanjTools/Level Tools/Align Selected Objects")]
        public static void AlignObjectsTool()
        {
            AlignObjects_Editor.Init(typeof(AlignObjects_Editor), EditorWindowData.EditorWindowInformations["Object Aligner"]);
        }

        #endregion

        #endregion

        #region Prefab Object EditorWindws

        #region Find Reference GameObjects
        [MenuItem("GursaanjTools/GameObject Tools/Find References")]
        public static void FindReferencesTool()
        {
            ObjectReferenceFinder_Editor.Init(typeof(ObjectReferenceFinder_Editor), EditorWindowData.EditorWindowInformations["GameObject Finder"]);
        }


        #endregion

        #endregion

        #region Debugging and Logging EditorWindows

        #region Selection Log
        [MenuItem("GursaanjTools/Logging Tools/Get Selection Log")]
        public static void GetSelectionLogTool()
        {
            SelectionLog_Editor.Init(typeof(SelectionLog_Editor), EditorWindowData.EditorWindowInformations["Selection Log"]);
        }


        #endregion


        #endregion

        #region Import EditorWindows

        #region Gist Importer
        
        [MenuItem("GursaanjTools/Import/Gist Importer")]
        public static void GistImporterTool()
        {
            GistImporter_Editor.Init(typeof(GistImporter_Editor), EditorWindowData.EditorWindowInformations["Gist Importer"]);
        }

        #endregion


        #endregion

        #region AssetMenuItem based EditorWindows

        #region Reference Finder

        [MenuItem("Assets/Find References", false, 1)]
        public static void FindReferences_AssetMenu_Tool()
        {
            ObjectReferenceFinder_Editor.AssetInit(EditorWindowData.EditorWindowInformations["GameObject Finder"]);
        }

        #endregion

        #endregion
    }
}

