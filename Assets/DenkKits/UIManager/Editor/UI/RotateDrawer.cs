
using DenkKits.UIManager.Scripts.UIAnimation;
using UnityEditor;
using UnityEngine;


using Imba.UI;

namespace Imba.Editor.UI
{
    [CustomPropertyDrawer(typeof(TweenRotate), true)]
    public class RotateDrawer : BaseAnimationDrawer
    {
      


        private void Init(SerializedProperty property)
        {
           
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

            Init(property);

            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);
            {
                // don't make child fields be indented
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 3;

                Color initialColor = GUI.color; //save the GUI color
                DrawSelector(position, property);
                GUI.color = initialColor; //restore the GUI color

                // set indent back to what it was
                EditorGUI.indentLevel = indent;

                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        protected override void DrawShow(Rect position, SerializedProperty property)
        {
           
            DrawLineStartDelayDurationCustomFromAndTo( property);

            SerializedProperty UseCustomFromAndTo = GetProperty(PropertyName.UseCustomFromAndTo, property);

            if (UseCustomFromAndTo.boolValue)
            {
                DrawProperty(PropertyName.From, property, "From");
                DrawProperty(PropertyName.To, property, "To");      

            }
            else
            {
                DrawProperty(PropertyName.From, property, "From");
            }
           
            DrawLineEaseTypeEaseAnimationCurveRotateMode( property);
        }

        protected override void DrawHide(Rect position, SerializedProperty property)
        {
          
            DrawLineStartDelayDurationCustomFromAndTo( property);

   
            SerializedProperty UseCustomFromAndTo = GetProperty(PropertyName.UseCustomFromAndTo, property);

            if (UseCustomFromAndTo.boolValue)
            {
                DrawProperty(PropertyName.To, property, "To");   
                DrawProperty(PropertyName.From, property, "From");                   
                
            }
            else
            {
                DrawProperty(PropertyName.From, property, "From");
            }

            DrawLineEaseTypeEaseAnimationCurveRotateMode( property);
        }

        protected override void DrawState(Rect position, SerializedProperty property)
        {
           
            DrawLineStartDelayAndDuration( property);

            DrawProperty(PropertyName.By, property, "By");
           
            DrawLineEaseTypeEaseAnimationCurveRotateMode( property);
        }

        protected override void DrawPunch(Rect position, SerializedProperty property)
        {
            DrawProperty(PropertyName.StartDelay, property, "StartDelay");
            DrawProperty(PropertyName.Duration, property, "Duration");
            DrawProperty(PropertyName.Vibrato, property, "Vibrato");
            DrawProperty(PropertyName.Elasticity, property, "Elasticity");
            DrawProperty(PropertyName.By, property, "By");
        }

        protected override void DrawLoop(Rect position, SerializedProperty property)
        {
            DrawProperty(PropertyName.StartDelay, property, "StartDelay");
            DrawProperty(PropertyName.Duration, property, "Duration");
            DrawProperty(PropertyName.NumberOfLoops, property, "NumberOfLoops");
            DrawProperty(PropertyName.LoopType, property, "LoopType");
            DrawProperty(PropertyName.By, property, "By");
            DrawProperty(PropertyName.RotateMode, property, "RotateMode");
            
            DrawLineEaseTypeEaseAnimationCurve( property);
        }

        protected override void DrawUndefined(Rect position, SerializedProperty property)
        {
            DrawProperty(PropertyName.AnimationType, property, "AnimationType");
        }
    }
}