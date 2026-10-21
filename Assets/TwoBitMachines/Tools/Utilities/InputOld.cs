using UnityEngine;

namespace TwoBitMachines
{
        public static class InputOld
        {
                public static bool GetKeyDown (KeyCode key)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetKeyDown(key);
#endif
                        return false;

                }

                public static bool GetMouseButtonDown (int button)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetMouseButtonDown(button);
#endif
                        return false;
                }

                public static bool AnyKeyDown ()
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.anyKeyDown;
#endif
                        return false;
                }

                public static int TouchCount ()
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.touchCount;
#endif
                        return 0;
                }

                public static float GetAxisRaw (string axisName)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetAxisRaw(axisName);
#endif
                        return 0f;
                }

                public static bool GetKey (KeyCode key)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetKey(key);
#endif
                        return false;
                }

                public static bool GetMouseButton (int button)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetMouseButton(button);
#endif
                        return false;
                }


                public static bool GetKeyUp (KeyCode key)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetKeyUp(key);
#endif
                        return false;
                }

                public static bool GetMouseButtonUp (int button)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetMouseButtonUp(button);
#endif
                        return false;
                }

                public static Vector3 MousePosition ()
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.mousePosition;
#endif
                        return Vector3.zero;
                }

                public static float MouseScrollDeltaY ()
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.mouseScrollDelta.y;
#endif
                        return 0f;
                }

                public static Touch? GetTouch (int index)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        if (Input.touchCount > index)
                                return Input.GetTouch(index);
                        return null;
#endif
                        return null;
                }

                public static float GetAxis (string axisName)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetAxis(axisName);
#endif
                        return 0f;
                }

                public static bool TouchPhaseBegan (int index = 0)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetTouch(index).phase == TouchPhase.Began;
#endif
                        return false;
                }

                public static bool TouchPhaseMoved (int index = 0)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetTouch(index).phase == TouchPhase.Moved;
#endif
                        return false;
                }

                public static Vector2 TouchDeltaPosition (int index = 0)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetTouch(index).deltaPosition;
#endif
                        return Vector2.zero;
                }

                public static Vector2 TouchPosition (int index = 0)
                {
#if ENABLE_LEGACY_INPUT_MANAGER
                        return Input.GetTouch(index).position;
#endif
                        return Vector2.zero;
                }
        }
}
