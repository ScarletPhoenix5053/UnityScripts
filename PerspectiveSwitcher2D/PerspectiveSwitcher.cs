using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sierra.PerspectiveSwitcher2D
{
    /// <summary>
    /// Main class for camera switching operation. Desgined to be attatched to a game manager component
    /// </summary>
    public class PerspectiveSwitcher : MonoBehaviour
    {
        public Camera.CameraController Camera;
        public CubePerspective CurrentPerspective;
        public BoolCube DeniedPerspectives = new BoolCube();
        public bool FreezeTimeOnSwtich;
        public float SmoothTransitionDuration = 0.5f;
        
        public enum CubePerspective
        {
            top,
            bottom,
            left,
            right,
            back,
            front
        }

        public void SetPerspective(CubePerspective newPerspective)
        {
            if (newPerspective == CurrentPerspective) return;
            if (!PerspectiveAllowed(newPerspective)) return;

            SetOrientationRefrenceTo(newPerspective);
            UpdateCamera();
        }
        public void DenyPerspective(BoolCube boolCube)
        {
            if (boolCube.Top) DeniedPerspectives.Top = true;
            if (boolCube.Bottom) DeniedPerspectives.Bottom = true;
            if (boolCube.Left) DeniedPerspectives.Left = true;
            if (boolCube.Right) DeniedPerspectives.Right = true;
            if (boolCube.Front) DeniedPerspectives.Front = true;
            if (boolCube.Bottom) DeniedPerspectives.Bottom = true;
        }
        public void DenyPerspective(CubePerspective newPerspective)
        {
            switch (newPerspective)
            {
                case CubePerspective.top:
                    DeniedPerspectives.Top = true;
                    break;
                case CubePerspective.bottom:
                    DeniedPerspectives.Bottom = true;
                    break;
                case CubePerspective.left:
                    DeniedPerspectives.Left = true;
                    break;
                case CubePerspective.right:
                    DeniedPerspectives.Right = true;
                    break;
                case CubePerspective.back:
                    DeniedPerspectives.Front = true;
                    break;
                case CubePerspective.front:
                    DeniedPerspectives.Bottom = true;
                    break;
                default:
                    break;
            }
        }
        public void AllowPerspective(BoolCube boolCube)
        {
            if (boolCube.Top) DeniedPerspectives.Top = false;
            if (boolCube.Bottom) DeniedPerspectives.Bottom = false;
            if (boolCube.Left) DeniedPerspectives.Left = false;
            if (boolCube.Right) DeniedPerspectives.Right = false;
            if (boolCube.Front) DeniedPerspectives.Front = false;
            if (boolCube.Bottom) DeniedPerspectives.Bottom = false;
        }
        public void AllowPerspective(CubePerspective newPerspective)
        {
            switch (newPerspective)
            {
                case CubePerspective.top:
                    DeniedPerspectives.Top = false;
                    break;
                case CubePerspective.bottom:
                    DeniedPerspectives.Bottom = false;
                    break;
                case CubePerspective.left:
                    DeniedPerspectives.Left = false;
                    break;
                case CubePerspective.right:
                    DeniedPerspectives.Right = false;
                    break;
                case CubePerspective.back:
                    DeniedPerspectives.Front = false;
                    break;
                case CubePerspective.front:
                    DeniedPerspectives.Bottom = false;
                    break;
                default:
                    break;
            }
        }

        private void UpdateCamera()
        {
            switch (CurrentPerspective)
            {/*
                case CubePerspective.top:
                    Camera.ArcInterpolateTo(Camera.LookAt.position + Vector3.up * Camera.DistToSubject, new Vector3(0,-1,1), SmoothTransitionDuration);
                    break;
                case CubePerspective.bottom:
                    Camera.ArcInterpolateTo(Camera.LookAt.position + Vector3.down * Camera.DistToSubject, SmoothTransitionDuration);
                    break;
                case CubePerspective.left:
                    Camera.ArcInterpolateTo(Camera.LookAt.position + Vector3.left * Camera.DistToSubject, SmoothTransitionDuration);
                    break;
                case CubePerspective.right:
                    Camera.ArcInterpolateTo(Camera.LookAt.position + Vector3.right * Camera.DistToSubject, SmoothTransitionDuration);
                    break;*/
                case CubePerspective.back:
                    Camera.PivotTo(Camera.LookAt.position + Vector3.back * Camera.DistToSubject, new Vector3(-0.1f, 0, 0), SmoothTransitionDuration);
                    break;
                case CubePerspective.front:
                    Camera.PivotTo(Camera.LookAt.position + Vector3.forward * Camera.DistToSubject, new Vector3(-0.1f, 0, 0), SmoothTransitionDuration);
                    break;
                default:
                    break;
            }
        }
        private bool PerspectiveAllowed(CubePerspective newPerspective)
        {
            switch (newPerspective)
            {
                case CubePerspective.top:
                    if (DeniedPerspectives.Top) return false;
                    break;
                case CubePerspective.bottom:
                    if (DeniedPerspectives.Bottom) return false;
                    break;
                case CubePerspective.left:
                    if (DeniedPerspectives.Left) return false;
                    break;
                case CubePerspective.right:
                    if (DeniedPerspectives.Right) return false;
                    break;
                case CubePerspective.back:
                    if (DeniedPerspectives.Back) return false;
                    break;
                case CubePerspective.front:
                    if (DeniedPerspectives.Front) return false;
                    break;
                default:
                    return false;
            }
            return true;
        }
        private void SetOrientationRefrenceTo(CubePerspective newPerspective)
        {
            CurrentPerspective = newPerspective;
        }
    }

    /// <summary>
    /// Six booleans named to represent the six faces of cube.
    /// </summary>
    [Serializable]
    public class BoolCube
    {
        /// <summary>
        /// Facing -z
        /// </summary>
        public bool Front = false;
        /// <summary>
        /// Facing +z
        /// </summary>
        public bool Back = false;
        /// <summary>
        /// Facing +x
        /// </summary>
        public bool Left = false;
        /// <summary>
        /// Facing -x
        /// </summary>
        public bool Right = false;
        /// <summary>
        /// Facing -y
        /// </summary>
        public bool Top = false;
        /// <summary>
        /// Facing +y
        /// </summary>
        public bool Bottom = false;

        public BoolCube()
        {
            Front = false;
            Back = false;
            Left = false;
            Right = false;
            Top = false;
            Bottom = false;
        }
    }
}