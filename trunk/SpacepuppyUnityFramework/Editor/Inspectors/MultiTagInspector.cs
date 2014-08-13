﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

using com.spacepuppy;

namespace com.spacepuppyeditor.Inspectors
{

    [CustomEditor(typeof(MultiTag))]
    public class MultiTagInspector : Editor
    {

        #region Properties

        public new MultiTag target { get { return base.target as MultiTag; } }

        private bool _showTags;

        #endregion

        public override void OnInspectorGUI()
        {
            //this may change in later releases...
            _showTags = EditorGUILayout.Foldout(_showTags, "Tags");

            if (_showTags)
            {
                var currentTags = this.target.GetTags().ToArray();
                var selectedTags = new List<string>();

                var tags = from tag in UnityEditorInternal.InternalEditorUtility.tags where (tag != SPConstants.TAG_UNTAGGED && tag != SPConstants.TAG_MULTITAG && tag != SPConstants.TAG_EDITORONLY) select tag;
                foreach (var tag in tags)
                {
                    var bSelected = currentTags.Contains(tag);
                    if (EditorGUILayout.Toggle(tag, bSelected))
                    {
                        selectedTags.Add(tag);
                    }
                }

                this.target.UpdateTags(selectedTags.ToArray());
            }

        }

    }

}