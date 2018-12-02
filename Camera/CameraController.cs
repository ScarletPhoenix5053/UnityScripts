using UnityEngine;
using System;
using System.Collections;

namespace Sierra.Camera
{
    public class CameraController : MonoBehaviour
    {
        public Transform LookAt;
        public float DistToSubject = 2;
        protected Vector3 DirectionToSubject { get { return LookAt.position - transform.position; } }
        /// <summary>
        /// Whether or not this camera is in the middle of a locked transition animation
        /// </summary>
        [HideInInspector]
        public bool Transitioning;

        protected virtual void Update()
        {

        }

        public virtual void TeleportTo(Vector3 destination)
        {
            transform.position = destination;
        }
        public virtual void InterpolateTo(Vector3 destination, float duration)
        {
            if(!Transitioning) StartCoroutine(IE_InterpolateTo(destination, duration, true));
        }
        /// <summary>
        /// THIS FUNCTION IS A WIP AND WILL THROW AN EXCEPTION
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="arcDirection"></param>
        /// <param name="duration"></param>
        public virtual void ArcInterpolateTo(Vector3 destination, Vector3 arcDirection, float duration)
        {
            throw new NotImplementedException("Arc Interpolation needs to be reconfigured.");
            //if (!Transitioning) StartCoroutine(IE_ArcInterpolateTo(destination, arcDirection, duration, true));
        }
        /// <summary>
        /// Pivot to a point in an arc motion. Limited to obtuse angles and does not work for 180 degree movements. 
        /// Use <see cref="ArcInterpolateTo(Vector3, Vector3, float)"/> or
        /// move the pivot point slightly so the transition is slightly les than 180 degrees.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="pivotPos"></param>
        /// <param name="duration"></param>
        public virtual void PivotTo(Vector3 destination, Vector3 pivotPos, float duration)
        {
            if (!Transitioning) StartCoroutine(IE_PivotTo(destination, pivotPos, duration, true));
        }

        protected virtual IEnumerator IE_InterpolateTo(Vector3 destination, float duration, bool transition = false)
        {
            if (transition) Transitioning = true;

            var timer = 0f;
            var originalPos = transform.position;

            // loop & interpolate till time is up
            while (timer <= duration)
            {
                transform.position = GetInterpolatedPos(originalPos, destination, timer / duration);

                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            // snap to destination
            transform.position = destination;
            Transitioning = false;

        }
        /// <summary>
        /// WIP. Need to fix arbitrary point calculation and allow non-circular arcs
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="arcDirection"></param>
        /// <param name="duration"></param>
        /// <param name="transition"></param>
        /// <returns></returns>
        protected virtual IEnumerator IE_ArcInterpolateTo(Vector3 destination, Vector3 arcDirection, float duration, bool transition = false)
        {
            if (transition) Transitioning = true;

            var timer = 0f;
            var originalPos = transform.position;

            // loop & interpolate till time is up
            while (timer <= duration)
            {
                var linearPos = GetInterpolatedPos(originalPos, destination, timer / duration);
                var halfPos = Vector3.Lerp(originalPos, destination, 0.5f);
                var inverseDir = arcDirection.normalized * ((originalPos - destination).magnitude/2);
                var inversePos = halfPos + inverseDir;
                var arcDir = (linearPos - inversePos).normalized;
                var arcPos = arcDir * (inversePos - originalPos).magnitude;
                transform.position = arcPos;

                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            // snap to destination
            transform.position = destination;
            Transitioning = false;

        }
        protected virtual IEnumerator IE_PivotTo(Vector3 destination, Vector3 pivotPos, float duration, bool transition = false)
        {
            if (transition) Transitioning = true;
            
            var timer = 0f;
            var originalPos = transform.position;
            var distFromOrigin = (pivotPos - originalPos).magnitude;
            var distFromDestination = (pivotPos - destination).magnitude;

            // loop & interpolate till time is up
            while (timer <= duration)
            {
                var linearPos = Vector3.Slerp(originalPos, destination, timer / duration);
                var pivotDir = (linearPos - pivotPos).normalized;
                var distToSubject = Mathf.SmoothStep(distFromDestination, distFromDestination, timer / duration);
                var camPos = pivotDir * distToSubject;
                transform.position = camPos;

                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            // snap to destination
            transform.position = destination;
            Transitioning = false;
        }

        protected Vector3 GetInterpolatedPos(Vector3 a, Vector3 b, float t)
        {
            var x = Mathf.SmoothStep(a.x, b.x, t);
            var y = Mathf.SmoothStep(a.y, b.y, t);
            var z = Mathf.SmoothStep(a.z, b.z, t);
            return new Vector3(x, y, z);
        }
    }   
}