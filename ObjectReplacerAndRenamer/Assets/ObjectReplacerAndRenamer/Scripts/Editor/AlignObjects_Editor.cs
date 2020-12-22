﻿using UnityEditor;
using UnityEngine;

namespace GursaanjTools
{
    public class AlignObjects_Editor : GuiControlEditorWindow
    {
        #region Variables
        
        //GUI Labels
        private const string ReferenceObjectLabel = "Object to Align to";
        private const string AlignLabel = "Align Selected Objects";
        private const string PositionLabel = "Position";
        private const string RotationLabel = "Rotation";
        private const string ScaleLabel = "Scale";
        private const string ReferenceValueLabel = "Reference Values";
        private const string XValueLabel = "X";
        private const string YValueLabel = "Y";
        private const string ZValueLabel = "Z";
        
        private const float VerticalComponentPadding = 1f;
        private const float HorizontalBorderPadding = 5f;
        private const float ValuePadding = 30f;
        private const float ComponentValuePadding = 50f;
        private const float ParentTogglePadding = 100f;
        
        private const string FloatLimitCast = "n2";
        
        //Warning Labels
        private const string NothingSelectedWarning = "At least one object from the hierarchy needs to be chosen to align";
        private const string NoReferenceObjectWarning = "Please select an Object to align to!";

        //Undo Labels
        private const string UndoAlignLabel = "Alignment";
        
        //rects
        private Rect _headerRect;
        private Rect _positionRect;
        private Rect _rotationRect;
        private Rect _scaleRect;
        
        private GameObject _referenceObject;
        private Transform _referenceTransform;
        private bool[] _positionAligners = new bool[3] {true, true, true};
        private bool[] _rotationAligners = new bool[3] {true, true, true};
        private bool[] _scaleAligners = new bool[3] {true, true, true};
        private bool _isPositionGroupEnabled = true;
        private bool _isRotationGroupEnabled = true;
        private bool _isScaleGroupEnabled = true;
        
        private GUIStyle _referenceObjectLabelStyle;
        #endregion
        
        #region BuiltIn Methods

        private void OnEnable()
        {
            _referenceObjectLabelStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                contentOffset = new Vector2(0, 3f)
            };
        }

        protected override void CreateGUI(string controlName)
        {
            _selectedGameObjects = Selection.gameObjects;
            CreateAreaRects();
            using (new EditorGUILayout.VerticalScope())
            {
                using (new GUILayout.AreaScope(_headerRect))
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            GUILayout.Label($"{SelectionCountString} {_selectedGameObjects.Length.ToString(CastedCountFormat)}");
                        }
                        
                        GUILayout.FlexibleSpace();

                        using (new EditorGUILayout.VerticalScope())
                        {
                            GUILayout.Label(ReferenceObjectLabel, _referenceObjectLabelStyle);
                            GUILayout.FlexibleSpace();
                            _referenceObject = (GameObject) EditorGUILayout.ObjectField(_referenceObject, typeof(GameObject), true, GUILayout.ExpandWidth(true));
                            GUILayout.FlexibleSpace();
                        }
                        
                        GUILayout.FlexibleSpace();
                    }
                }

                if (_referenceObject != null)
                {
                    _referenceTransform = _referenceObject.transform;

                    GUILayout.FlexibleSpace();
                
                    CreateComponentScope(_positionRect, ref _isPositionGroupEnabled, ref _positionAligners, PositionLabel, _referenceTransform.position);
                    CreateComponentScope(_rotationRect, ref _isRotationGroupEnabled, ref _rotationAligners, RotationLabel, _referenceTransform.rotation.eulerAngles);
                    CreateComponentScope(_scaleRect, ref _isScaleGroupEnabled, ref _scaleAligners, ScaleLabel, _referenceTransform.lossyScale);

                    if (GUILayout.Button(AlignLabel, GUILayout.ExpandHeight(true)) || IsReturnPressed())
                    {
                        AlignObjects();
                    } 
                }
            }
        }
        #endregion

        #region Custom Methods

        private void AlignObjects()
        {
            if (_selectedGameObjects == null || _selectedGameObjects.Length == 0)
            {
                DisplayDialogue(ErrorTitle, NothingSelectedWarning, false);
                return;
            }

            if (_referenceObject == null)
            {
                DisplayDialogue(ErrorTitle, NoReferenceObjectWarning, false);
                return;
            }

            Transform referenceTransform = _referenceObject.transform;
            
            foreach (GameObject obj in _selectedGameObjects)
            {
                if (obj == null)
                {
                    continue;
                }
                
                Undo.RecordObject(obj.transform, UndoAlignLabel);
                
                obj.transform.position = GetAlignedVector(_isPositionGroupEnabled, _positionAligners,
                    obj.transform.position, referenceTransform.position);
                
                obj.transform.rotation = Quaternion.Euler(GetAlignedVector(_isRotationGroupEnabled, _rotationAligners,
                    obj.transform.rotation.eulerAngles, referenceTransform.rotation.eulerAngles));

                obj.transform.localScale = GetAlignedVector(_isScaleGroupEnabled, _scaleAligners,
                    obj.transform.localScale, referenceTransform.transform.localScale);
            }
            
        }

        private void CreateAreaRects()
        {
            _headerRect = new Rect(0,0, position.width, position.height/6);
            _positionRect = new Rect(0, 50, position.width, position.height);
            _rotationRect = new Rect(0, 100, position.width, position.height);
            _scaleRect = new Rect(0, 150, position.width, position.height);
        }

        private void CreateComponentScope(Rect areaRect, ref bool parentToggle, ref bool[] toggleArray, string parentLabel, Vector3 referenceVector)
        {
            using (new GUILayout.AreaScope(areaRect))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.Space(HorizontalBorderPadding);
                    using (new EditorGUILayout.VerticalScope())
                    {
                        CreateToggleGroup(ref parentToggle, ref toggleArray, parentLabel);
                    }
                        
                    using (new EditorGUILayout.VerticalScope())
                    {
                        if (_referenceObject != null)
                        {
                            EditorGUILayout.Space(VerticalComponentPadding);
                            CreateTransformComponentLabels(referenceVector);
                        }
                    }
                }
            }
                
            GUILayout.FlexibleSpace();
        }

        // Didn't use BeginToggleGroup as it can display grouping vertically (not the desired Horizontally)
        private void CreateToggleGroup(ref bool parentToggle, ref bool[] toggleArray, string parentLabel)
        {
            if (toggleArray == null || toggleArray.Length != 3 || string.IsNullOrEmpty(parentLabel))
            {
                Debug.LogWarning("Cannot Create Toggle Group");
            }
            
            parentToggle = EditorGUILayout.ToggleLeft(parentLabel, parentToggle, EditorStyles.boldLabel, GUILayout.Width(ParentTogglePadding));

            GUI.enabled = parentToggle;

            using (new EditorGUILayout.HorizontalScope())
            {
                toggleArray[0] = EditorGUILayout.ToggleLeft(XValueLabel, toggleArray[0], GUILayout.Width(ValuePadding));
                toggleArray[1] = EditorGUILayout.ToggleLeft(YValueLabel, toggleArray[1], GUILayout.Width(ValuePadding));
                toggleArray[2] = EditorGUILayout.ToggleLeft(ZValueLabel, toggleArray[2], GUILayout.Width(ValuePadding));
                GUILayout.FlexibleSpace();
            }
            GUI.enabled = true;
        }

        private void CreateTransformComponentLabels(Vector3 componentVector)
        {
            GUILayout.Label(ReferenceValueLabel, EditorStyles.boldLabel);
            
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField($"{XValueLabel}: {componentVector.x.ToString(FloatLimitCast)}", GUILayout.Width(ComponentValuePadding));
                EditorGUILayout.LabelField($"{YValueLabel}: {componentVector.y.ToString(FloatLimitCast)}", GUILayout.Width(ComponentValuePadding));
                EditorGUILayout.LabelField($"{ZValueLabel}: {componentVector.z.ToString(FloatLimitCast)}", GUILayout.Width(ComponentValuePadding));
                GUILayout.FlexibleSpace();
            }
        }

        private Vector3 GetAlignedVector(bool parentToggle, bool[] toggleArray, Vector3 objVector, Vector3 refVector)
        {
            Vector3 newVector = objVector;

            if (toggleArray != null && toggleArray.Length == 3 && parentToggle)
            {
                if (toggleArray[0])
                {
                    newVector.x = refVector.x;
                }
                
                if (toggleArray[1])
                {
                    newVector.y = refVector.y;
                }

                if (toggleArray[2])
                {
                    newVector.z = refVector.z;
                }
            }

            return newVector;
        }

        #endregion
    }
}
