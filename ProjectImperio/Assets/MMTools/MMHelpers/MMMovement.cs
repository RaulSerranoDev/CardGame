using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace MoreMountains.Tools
{	
	/// <summary>
	/// Various static methods used throughout the Infinite Runner Engine and the Corgi Engine.
	/// </summary>

	public static class MMMovement 
	{

		/// <summary>
		/// Moves an object from point A to point B in a given time
		/// </summary>
		/// <param name="movingObject">Moving object.</param>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="time">Time.</param>
		public static IEnumerator MoveFromTo(GameObject movingObject,Vector3 pointA, Vector3 pointB, float time, float approximationDistance)
		{
			float t = 0f;
	        
	        float distance = Vector3.Distance(movingObject.transform.position, pointB);

			while (distance >= approximationDistance)
			{
	            distance = Vector3.Distance(movingObject.transform.position, pointB);
				t += Time.deltaTime / time; 
				movingObject.transform.position = Vector3.Lerp(pointA, pointB, t);
				yield return 0;
			}
	        yield break;
		}

        /// <summary>
		/// Moves an object from point A to point B in a given time
		/// </summary>
		/// <param name="movingObject">Moving object.</param>
		/// <param name="pointA">Point a.</param>
		/// <param name="pointB">Point b.</param>
		/// <param name="time">Time.</param>
		public static IEnumerator MoveLocalFromTo(GameObject movingObject, Vector3 pointA, Vector3 pointB, float time, float approximationDistance)
        {
            float t = 0f;

            float distance = Vector3.Distance(movingObject.transform.localPosition, pointB);

            while (distance >= approximationDistance)
            {
                distance = Vector3.Distance(movingObject.transform.localPosition, pointB);
                t += Time.deltaTime / time;
                movingObject.transform.localPosition = Vector3.Lerp(pointA, pointB, t);
                yield return 0;
            }
            yield break;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotatingObject"></param>
        /// <param name="angle"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static IEnumerator RotateObject(Transform rotatingObject, float angle, float speed)
        {
            Vector3 toRotation = rotatingObject.localEulerAngles + new Vector3(0, 0, angle);

            while (Mathf.Abs(Mathf.DeltaAngle(rotatingObject.localEulerAngles.z, toRotation.z)) > 0.01f)
            {
                rotatingObject.rotation = Quaternion.RotateTowards(rotatingObject.rotation, Quaternion.Euler(toRotation), Time.deltaTime * speed);
                yield return null;
            }
            rotatingObject.localEulerAngles = toRotation;
        }
    }
}