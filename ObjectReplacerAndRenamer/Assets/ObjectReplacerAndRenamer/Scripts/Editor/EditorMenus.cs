﻿using UnityEditor;

namespace GursaanjTools
{
    public static class EditorMenus
    {
        #region Object EditorWindows

        #region Object Replacer Tool
        
        [MenuItem("GursaanjTools/GameObject Tools/Replace Selected Objects")]
        public static void ReplaceObjectsTool()
        {
            ReplaceObjects_Editor.Init(typeof(ReplaceObjects_Editor), EditorWindowData.EditorWindowInformations["Object Replacer"]);
        }

        #endregion

        #region Object Renamer

        [MenuItem("GursaanjTools/GameObject Tools/Rename Selected Objects")]
        public static void RenameObjectsTool()
        {
            RenameObjects_Editor.Init(typeof(RenameObjects_Editor), EditorWindowData.EditorWindowInformations["Object Renamer"]);
        }

        #endregion

        #region Object Grouper

        [MenuItem("GursaanjTools/GameObject Tools/Group Selected Objects %#z")]
        public static void GroupObjectsTool()
        {
            GroupObjects_Editor.Init(typeof(GroupObjects_Editor), EditorWindowData.EditorWindowInformations["Object Grouper"]);
        }

        #endregion

        #region Object Ungrouper
        
        [MenuItem("GursaanjTools/GameObject Tools/Ungroup Selected Objects %#q")]
        public static void UngroupObjectsTool()
        {
            UngroupObjects_Editor.Init();
        }

        #endregion

        #region Object Aligner

        [MenuItem("GursaanjTools/GameObject Tools/Align Selected Objects")]
        public static void AlignObjectsTool()
        {
            AlignObjects_Editor.Init(typeof(AlignObjects_Editor), EditorWindowData.EditorWindowInformations["Object Aligner"]);
        }

        #endregion

        #endregion
    }
}

