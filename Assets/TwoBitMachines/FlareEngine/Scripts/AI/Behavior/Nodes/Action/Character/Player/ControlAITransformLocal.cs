#region 
#if UNITY_EDITOR
using TwoBitMachines.Editors;
using UnityEditor;
#endif
#endregion
using System.Collections.Generic;
using TwoBitMachines.FlareEngine.ThePlayer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoBitMachines.FlareEngine.AI
{
        [AddComponentMenu("")]
        public class ControlAITransformLocal : Action
        {
                [SerializeField] public List<Vector3> position = new List<Vector3>();
                [SerializeField] public AIBase ai;
                [SerializeField] public UnityEvent onComplete;
                [SerializeField] public bool mirrorX;
                [SerializeField] public string animationSignal;

                [System.NonSerialized] private float directionX;
                [System.NonSerialized] private int index = 0;
                [System.NonSerialized] private float duration;
                [System.NonSerialized] private float counter = 0;
                [System.NonSerialized] private Vector2 startPosition;
                [System.NonSerialized] private Vector2 playerPosition;
                [System.NonSerialized] private Vector2 targetPosition;
                [System.NonSerialized] private CharacterType previousType;
                [System.NonSerialized] private bool initialized = false;

                public override NodeState RunNodeLogic (Root root)
                {
                        if (ai == null || position.Count == 0)
                                return NodeState.Failure;

                        if (nodeSetup == NodeSetup.NeedToInitialize)
                        {
                                index = 0;
                                counter = 0;
                                initialized = true;
                                previousType = ai.type;
                                ai.type = CharacterType.NoCollisionChecks;
                                directionX = ai.root.direction;
                                playerPosition = ai.transform.position;
                                startPosition = playerPosition;
                                targetPosition = Position(out duration);
                        }

                        MovePlayer();

                        if (Mathf.Clamp01(counter / duration) >= 1f)
                        {
                                index++;
                                counter = 0;
                                playerPosition = targetPosition;
                                targetPosition = Position(out duration);
                        }
                        if (index >= position.Count)
                        {
                                Invoke(root);
                                ai.type = previousType;
                                return NodeState.Success;
                        }
                        ai.signals.Set(animationSignal);

                        return NodeState.Running;
                }

                public override void OnReset (bool skip = false, bool enteredState = false)
                {
                        if (initialized)
                        {
                                ai.type = previousType;
                                initialized = false;
                        }
                }

                private Vector2 Position (out float duration)
                {
                        duration = 1;
                        if (index >= position.Count)
                                return Vector3.zero;
                        Vector2 localPosition = position[index];
                        localPosition.x = mirrorX ? Mathf.Sign(directionX) * localPosition.x : localPosition.x;
                        duration = position[index].z;
                        return localPosition + startPosition;
                }

                private void MovePlayer ()
                {
                        counter += Time.deltaTime;

                        // Compute constant velocity based on your existing data
                        Vector2 start = playerPosition;
                        Vector2 end = targetPosition;

                        float distance = Vector2.Distance(start, end);
                        float speed = distance / duration;

                        // Direction from current AI position toward the final point
                        Vector2 pos = ai.transform.position;
                        Vector2 dir = (end - pos).normalized;

                        // Constant-speed intended velocity this frame
                        Vector2 velocity = dir * speed;// / Time.deltaTime;

                        // Move through your world system
                        ai.world.box.Update();
                        ai.world.Move(ref velocity, ref ai.velocity.y, false, Time.deltaTime, ref ai.root.onSurface);

                        // You were zeroing these, so keep the behavior
                        ai.velocity.y = 0;

                        SetVelSignals(velocity, ai.signals);
                }

                private void SetVelSignals (Vector2 velocity, AnimationSignals signals)
                {
                        bool velX = velocity.x != 0;
                        bool velXLeft = velocity.x < 0;
                        bool velXRight = velocity.x > 0;
                        bool velXZero = velocity.x == 0;
                        bool velY = velocity.y != 0;
                        bool velYUp = velocity.y > 0;
                        bool velYDown = velocity.y < 0;
                        bool velYZero = velocity.y == 0;

                        signals.Set("velX", velX);
                        signals.Set("velXLeft", velXLeft);
                        signals.Set("velXRight", velXRight);
                        signals.Set("velXZero", velXZero);
                        signals.Set("velY", velY);
                        signals.Set("velYUp", velYUp);
                        signals.Set("velYDown", velYDown);
                        signals.Set("velYZero", velYZero);

                        if (velocity.x != 0)
                                signals.SetDirection((int) Mathf.Sign(velocity.x));
                }

                private void Invoke (Root root)
                {
                        ai.signals.SetDirection(root.direction);
                        onComplete.Invoke();
                }

                #region ▀▄▀▄▀▄ Custom Inspector ▄▀▄▀▄▀
#if UNITY_EDITOR
#pragma warning disable 0414
                [SerializeField, HideInInspector] public bool eventFoldout;
                public override bool OnInspector (AIBase ai, SerializedObject parent, Color color, bool onEnable)
                {
                        if (parent.Bool("showInfo"))
                        {
                                Labels.InfoBoxTop(105, "Move the player's transform along a set of points. These points are local to the Player. The Z value is the time duration. This will block player input and set a player animation signal." +
                                        "\n \nReturns Running, Success, Failure");
                        }

                        SerializedProperty array = parent.Get("position");
                        if (array.arraySize == 0)
                        {
                                array.arraySize++;
                        }

                        FoldOut.Box(3 + array.arraySize, color, offsetY: -2);
                        {
                                parent.Field("AI", "ai");
                                parent.Field("Animation Signal", "animationSignal");
                                parent.Field("Mirror X Position", "mirrorX");
                                for (int i = 0; i < array.arraySize; i++)
                                {
                                        Fields.ArrayProperty(parent.Get("position"), array.Element(i), i, "Local Position");
                                }
                        }
                        Layout.VerticalSpacing(3);
                        Fields.EventFoldOut(parent.Get("onComplete"), parent.Get("eventFoldout"), "On Complete", color: color);
                        return true;
                }
#pragma warning restore 0414
#endif
                #endregion
        }
}