using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using System.IO;
using UnityEditor;
using UnityEngine.Rendering;
using System.Drawing;
using JetBrains.Annotations;
using static UnityEditor.Progress;
using UnityEngine.UI;
using System.Xml.Linq;
using System.Text;
using UnityEditor.Experimental.GraphView;
using static UnityEditor.PlayerSettings;
using Unity.VisualScripting;

using System.Xml;                   //basic xml attribute
using System.Xml.Serialization;     //access xml serializer
using System.IO;                    //file management
using System.Runtime.Serialization.Formatters.Binary;

public static class Utility
{
    #region Layer/Tags

    public static LayerMask[] GetAllLayerMasks()
    {
        int totalLayers = 32; // Unity supports up to 32 layers
        LayerMask[] layerMasks = new LayerMask[totalLayers];

        for (int i = 0; i < totalLayers; i++)
        {
            string layerName = LayerMask.LayerToName(i);
            layerMasks[i] = LayerMask.GetMask(layerName);
        }

        return layerMasks;
    }

    public static string[] GetAllTags()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProperty = tagManager.FindProperty("tags");

        string[] tags = new string[tagsProperty.arraySize];

        for (int i = 0; i < tagsProperty.arraySize; i++)
        {
            tags[i] = tagsProperty.GetArrayElementAtIndex(i).stringValue;
        }

        return tags;
    }

    public static LayerMask CombineLayerMasks(params LayerMask[] layerMasks)
    {
        LayerMask combinedLayerMask = 0;

        foreach (LayerMask layerMask in layerMasks)
        {
            combinedLayerMask |= layerMask;
        }

        return combinedLayerMask;
    }

    #endregion

    #region Inputs

    public static Vector2 GetKeyDirection()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    public static Vector2 GetMouseDirection()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
    public static float GetMouseScroll()
    {
        return Input.GetAxis("Mouse ScrollWheel");
    }


    #endregion

    #region Objects

    public static T CheckObject<T>(T obj) where T : new()
    {
        if (obj == null)
        {
            obj = new T();
        }

        return obj;
    }

    public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        //        lineRenderer = Utility.GetOrAddComponent<LineRenderer>(gameObject);
        T component = gameObject.GetComponent<T>();

        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    public static bool Objectxist(string Name)
    {

        GameObject myObject = GameObject.Find(Name);

        if (myObject == null)
        {
            return false;
        }
        return true;

    }
    public static bool ObjectxistPartial(string Name)
    {

        GameObject foundObject = GameObject.Find(Name);

        if (foundObject == null)
        {
            // Object with exact name not found, search for objects with at least the same letters
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains(Name))
                {
                    return true;
                }
            }


        }
        else
        {
            return true;

        }

        return false;
    }

    public static GameObject GetorCreateObject(string Name)
    {
        GameObject myObject = GameObject.Find(Name);

        if (myObject == null)
        {
            myObject = new GameObject(Name);
        }
        return myObject;
    }

    public static GameObject GetOrCreateObjectPartialName(string objectName)
    {
        GameObject foundObject = GameObject.Find(objectName);

        if (foundObject == null)
        {
            // Object with exact name not found, search for objects with at least the same letters
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains(objectName))
                {
                    foundObject = obj;
                    break;
                }
            }

            if (foundObject == null)
            {
                // No object with at least the same letters found, create a new one
                foundObject = new GameObject(objectName);
            }
        }

        return foundObject;
    }

    public static GameObject GetorCreateChild(GameObject parent, string Name)
    {
        if (parent.transform.Find(Name) != null)
        {
            return parent.transform.Find(Name).gameObject;
        }
        else
        {
            GameObject obj = new GameObject(Name);
            obj.transform.SetParent(parent.transform);

            return obj;
        }

    }

    public static GameObject GetOrCreateObjectPartialName(GameObject parent, string Name)
    {
        GameObject foundObject = parent.transform.Find(Name).gameObject;

        if (foundObject == null)
        {
            // Object with exact name not found, search for objects with at least the same letters

            foreach (Transform obj in parent.transform)
            {
                if (obj.name.Contains(Name))
                {
                    foundObject = obj.gameObject;
                    break;
                }
            }

            if (foundObject == null)
            {
                // No object with at least the same letters found, create a new one
                foundObject = new GameObject(Name);
                foundObject.transform.SetParent(parent.transform);
            }
        }

        return foundObject;
    }


    #endregion

    #region RectTransform
    public static void MoveToTopRightCanvas(RectTransform rectTransform, GameObject canvas)
    {
        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();

        // Get the size of the canvas
        Vector2 canvasSize = canvasRectTransform.sizeDelta;

        // Get the size of the RectTransform
        Vector2 rectTransformSize = rectTransform.sizeDelta;

        // Calculate the position for the top-right corner of the RectTransform
        Vector2 newPosition = new Vector2(canvasSize.x / 2f - rectTransformSize.x / 2f, canvasSize.y / 2f - rectTransformSize.y / 2f);

        // Set the new anchored position for the RectTransform
        rectTransform.anchoredPosition = newPosition;
    }

    public static void SetAnchorPreset(AnchorPreset anchorPreset, RectTransform rectTransform)
    {
        switch (anchorPreset)
        {
            case AnchorPreset.TopLeft:
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                rectTransform.pivot = new Vector2(0f, 1f);

                break;
            case AnchorPreset.TopCenter:
                rectTransform.anchorMin = new Vector2(0.5f, 1f);
                rectTransform.anchorMax = new Vector2(0.5f, 1f);
                rectTransform.pivot = new Vector2(0.5f, 1f);
                break;
            case AnchorPreset.TopRight:
                rectTransform.anchorMin = new Vector2(1f, 1f);
                rectTransform.anchorMax = new Vector2(1f, 1f);
                rectTransform.pivot = new Vector2(1f, 1f);
                break;
            case AnchorPreset.MiddleLeft:
                rectTransform.anchorMin = new Vector2(0f, 0.5f);
                rectTransform.anchorMax = new Vector2(0f, 0.5f);
                rectTransform.pivot = new Vector2(0f, 0.5f);
                break;
            case AnchorPreset.MiddleCenter:
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                break;
            case AnchorPreset.MiddleRight:
                rectTransform.anchorMin = new Vector2(1f, 0.5f);
                rectTransform.anchorMax = new Vector2(1f, 0.5f);
                rectTransform.pivot = new Vector2(1f, 0.5f);
                break;
            case AnchorPreset.BottomLeft:
                rectTransform.anchorMin = new Vector2(0f, 0f);
                rectTransform.anchorMax = new Vector2(0f, 0f);
                rectTransform.pivot = new Vector2(0f, 0f);
                break;
            case AnchorPreset.BottomCenter:
                rectTransform.anchorMin = new Vector2(0.5f, 0f);
                rectTransform.anchorMax = new Vector2(0.5f, 0f);
                rectTransform.pivot = new Vector2(0.5f, 0f);
                break;
            case AnchorPreset.BottomRight:
                rectTransform.anchorMin = new Vector2(1f, 0f);
                rectTransform.anchorMax = new Vector2(1f, 0f);
                rectTransform.pivot = new Vector2(1f, 0f);
                break;
        }
        rectTransform.anchoredPosition = Vector2.zero;

    }

    public static Vector2 GetMouseAnchor(Canvas canvas, Camera cam)
    {
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out localPosition);

        return localPosition;
    }

    public static Vector2 GetRectSize(RectTransform rectTransform)
    {
        return new Vector2(rectTransform.rect.width, rectTransform.rect.height);
    }
    public static Vector2 GetRectPositionMax(RectTransform rectTransform)
    {
        return new Vector2(rectTransform.rect.xMax, rectTransform.rect.yMax);
    }
    public static Vector2 GetRectPositionMin(RectTransform rectTransform)
    {
        return new Vector2(rectTransform.rect.xMin, rectTransform.rect.yMin);
    }

    public static Vector2 GetScreenSize()
    {
        return new Vector2(Screen.width, Screen.height);
    }

    public static void GetRectCorners(RectTransform rect, out Vector2 upperleft, out Vector2 upperright, out Vector2 lowerleft, out Vector2 lowerright)
    {
        Vector2 max = Utility.GetRectSize(rect);

        upperleft = new Vector2(rect.anchoredPosition.x - max.x, rect.anchoredPosition.y + max.y);
        upperright = new Vector2(rect.anchoredPosition.x + max.x, rect.anchoredPosition.y + max.y);
        lowerleft = new Vector2(rect.anchoredPosition.x - max.x, rect.anchoredPosition.y - max.y);
        lowerright = new Vector2(rect.anchoredPosition.x + max.x, rect.anchoredPosition.y - max.y);
    }
    public static void GetCanvasCorners(RectTransform rect, out Vector2 upperleft, out Vector2 upperright, out Vector2 lowerleft, out Vector2 lowerright)
    {
        //        canvas.GetWorldCorners(arrays);

        Vector2 max = Utility.GetRectSize(rect);

        upperleft = new Vector2(0, max.y) - rect.anchoredPosition;
        upperright = new Vector2(max.x, max.y) - rect.anchoredPosition;
        lowerleft = new Vector2(0, 0) - rect.anchoredPosition;
        lowerright = new Vector2(max.x, 0) - rect.anchoredPosition;
    }
    public static Vector3 GetRectWorldPosition(RectTransform rectTransform, Canvas canvas)
    {

        // Calculate the position of the anchored position in screen space
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rectTransform.position);

        // Convert the screen position to world space
        Vector3 worldPosition = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPosition, canvas.worldCamera, out worldPosition);

        return worldPosition;
    }
    public static Vector2 ScaleProportion(Vector2 scaleold, Vector2 scalenew, Vector2 scalingsize)
    {
        // Calculate the scale factors
        float widthScaleFactor = scalenew.x / scaleold.x;
        float heightScaleFactor = scalenew.y / scaleold.y;

        // Scale down image 3 proportionally
        float newWidth = (scalingsize.x * widthScaleFactor);
        float newHeight = (scalingsize.y * heightScaleFactor);

        return new Vector2(newWidth, newHeight);
    }

    public static Vector3 ConvertAnchoredPositionToWorldSpace(Camera cam, Canvas canvas, Vector2 anchoredPosition)
    {
        // Get the canvas scale factor
        Vector2 canvasScale = canvas.transform.localScale;

        // Calculate the scaled anchored position
        Vector2 scaledAnchoredPosition = new Vector2(anchoredPosition.x * canvasScale.x, anchoredPosition.y * canvasScale.y);

        // Calculate the position in screen space
        Vector2 screenPosition = canvas.transform.TransformPoint(scaledAnchoredPosition);

        // Convert the screen position to world space
        Vector3 worldPosition = cam.ScreenToWorldPoint(screenPosition);

        return worldPosition;
    }

    public static Vector3 ConvertAnchoredPositionToPosition(RectTransform rectTransform)
    {
        // Convert anchored position to position in local space
        Vector3 localPosition = rectTransform.TransformPoint(rectTransform.anchoredPosition);

        // Convert local position to position in world space
        Vector3 worldPosition = rectTransform.parent.TransformPoint(localPosition);

        return worldPosition / 2;
    }

    public static Vector2 ConvertAnchoredPositionToCanvasSpace(RectTransform rectTransform, Canvas canvas)
    {
        // Get the anchor position relative to the canvas pivot
        Vector2 canvasPivot = canvas.GetComponent<RectTransform>().pivot;
        Vector2 anchorPosition = rectTransform.anchoredPosition;
        Vector2 canvasSpacePosition = new Vector2(
            anchorPosition.x + canvasPivot.x * rectTransform.rect.width,
            anchorPosition.y + canvasPivot.y * rectTransform.rect.height
        );

        return canvasSpacePosition;
    }
    #endregion

    #region Camera
    public static bool IsPositionOnEdge(Vector3 position, float edgeThreshold)
    {
        // Get the screen width and height
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Define the edge threshold in pixel units
        float edgeThresholdPixels = edgeThreshold;

        // Check if the position is within the edge threshold from any side of the screen
        return position.x <= edgeThresholdPixels ||
               position.x >= screenWidth - edgeThresholdPixels ||
               position.y <= edgeThresholdPixels ||
               position.y >= screenHeight - edgeThresholdPixels;
    }

    public static Vector3 CalculateEdgeDirection(Camera cam, Vector3 mousePosition, Vector3 targetPosition)
    {
        // Convert the mouse position from screen coordinates to world coordinates
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, cam.nearClipPlane));

        // Calculate the direction towards the target object
        Vector3 direction = targetPosition - mouseWorldPosition;
        direction.Normalize();

        return direction;
    }

    #endregion

    #region Calculations
    public static float AddToProgress(float currentValue, float valueToAdd, float min, float max)
    {
        float clampedValue = Mathf.Clamp(currentValue + valueToAdd, min, max);
        return clampedValue;
    }
    public static float GetAngleXZToPoint(Transform objects, Vector3 target)
    {
        Vector3 forwardDirection = objects.transform.forward;
        Vector3 targetDirection = (target - objects.transform.position).normalized;

        return Vector3.SignedAngle(new Vector3(forwardDirection.x, 0, forwardDirection.z), new Vector3(targetDirection.x, 0, targetDirection.z), Vector3.up);
    }

    public static float GetAngleYZToPoint(Transform objects, Vector3 target)
    {
        Vector3 forwardDirection = objects.transform.forward;
        Vector3 targetDirection = (target - objects.transform.position).normalized;

        return Vector3.SignedAngle(new Vector3(0, forwardDirection.y, forwardDirection.z), new Vector3(0, targetDirection.y, targetDirection.z), Vector3.right);
    }

    public static float GetSquaredDistanceFromPointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 lineDirection = lineEnd - lineStart;
        float lineLengthSquared = lineDirection.sqrMagnitude;
        if (lineLengthSquared == 0f)
        {
            // If the line has zero length, return the squared distance to the start point
            return (point - lineStart).sqrMagnitude;
        }

        // Calculate the parameter along the line where the closest point to the point lies
        float t = Vector3.Dot(point - lineStart, lineDirection) / lineLengthSquared;

        // Clamp the parameter to ensure the closest point lies within the line segment
        t = Mathf.Clamp01(t);

        // Calculate the closest point on the line
        Vector3 closestPoint = lineStart + t * lineDirection;

        // Calculate the squared distance from the point to the closest point on the line
        return (point - closestPoint).sqrMagnitude;
    }

    public static int ReturnClosestLine(Vector3[,] lines, Vector3 point)
    {
        List<float> distances = new List<float>();
        for (int i = 0; i < 4; i++)
        {
            distances.Add(GetSquaredDistanceFromPointToLine(point, lines[i, 0], lines[i, 1]));
        }

        // Compare the distances to find the closest line
        float minDistance = distances.Min();
        for (int i = 0; i < distances.Count; i++)
        {
            if (minDistance == distances[i])
            {
                return i;
            }
        }
        return -1;
    }

    public static bool Timer(ref float countdown)
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0) return true;
        else return false;
    }
    #endregion

    #region AI
    public static float PrepareFire(UnitAI AI, Transform Unit, Transform target, Transform barrel, float EffectiveRange, float Firerate, float minAngle, float nextfire)
    {
        if (barrel != null)
        {
            float dist = Vector3.Distance(Unit.transform.position, target.transform.position);
            if (dist < EffectiveRange)
            {
                //head.LookAt(target.transform.position);
                AI.Aim(target.transform.position);
                Vector3 targetDir = target.transform.position - barrel.transform.position;
                //targetDir.y = barrel.transform.position.y; // set y to zero so we only consider the XZ plane
                float angle = Vector3.Angle(targetDir, barrel.transform.forward);
                // target is within angle threshold

                if (angle < 5f)
                {
                    if (Time.time > nextfire)
                    {
                        nextfire = Time.time + 1f / (Firerate / 60);//60= rate per minutes
                                                                    //  Shoot();

                        AI.SpawnProjectile();


                    }
                }
            }
        }
        return nextfire;
    }

    public static void MoveAgent(NavMeshAgent agent, Transform body, Vector3 waypoint, float smoothTime, bool reverse)
    {
        if (!reverse)
        {
            agent.SetDestination(waypoint);

            Vector3 direction = waypoint - body.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            targetRotation.x = 0;
            targetRotation.z = 0;
            // Interpolate between the current and target rotation
            body.transform.rotation = Quaternion.Lerp(body.transform.rotation, targetRotation, smoothTime * Time.deltaTime);
        }
        else
        {
            agent.SetDestination(waypoint);

            Vector3 direction = body.transform.position - waypoint;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            targetRotation.x = 0;
            targetRotation.z = 0;
            // Interpolate between the current and target rotation
            body.transform.rotation = Quaternion.Lerp(body.transform.rotation, targetRotation, smoothTime * Time.deltaTime);
        }
    }

    public static Vector3[] CreatePathFromAgent(NavMeshAgent agent)
    {
        if (agent.path != null && agent.path.corners.Length > 0)
        {
            return agent.path.corners;
            // Do something with the path points...
        }
        else
            return null;
    }

    public static void RenderLinerendererPath(NavMeshPath path, LineRenderer lineRenderer)
    {
        Vector3[] pathPoints = path.corners;

        lineRenderer.positionCount = pathPoints.Length;
        lineRenderer.SetPositions(ReverseArray(pathPoints));
    }

    public static void ClearLinerenderPath(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 0;
    }
    public static void RenderAgentPath(NavMeshAgent agent, LineRenderer lineRenderer)
    {

        if (agent != null && agent.hasPath)
        {
            Utility.RenderLinerendererPath(agent.path, lineRenderer);
        }
        else
        {
            Utility.ClearLinerenderPath(lineRenderer);
        }
    }

    #endregion

    #region Polynomial Interpolations

    #region Vectors
    public static Vector3 Linear(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, t);
    }

    public static Vector3 Squared(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, t * t);
    }

    public static Vector3 Cube(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, t * t * t);
    }

    public static Vector3 SquareRoot(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, Mathf.Sqrt(t));
    }

    public static Vector3 SmoothStep(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));
    }

    public static Vector3 QuadraticEaseOut(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, 1f - Mathf.Pow(1f - t, 2));
    }

    public static Vector3 Parabola(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, 4f * t * (1f - t));
    }

    public static Vector3 Triangle(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, Mathf.PingPong(t, 1f));
    }

    public static Vector3 ElasticOut(Vector3 start, Vector3 end, float t)
    {
        float amplitude = 1f;
        float period = 0.3f;
        float overshoot = 1.70158f;

        t = Mathf.Clamp01(t);
        float x = t - 1f;
        float exponent = Mathf.Exp(overshoot * x);
        float sine = Mathf.Sin(period * Mathf.PI * t);

        return start + (end - start) * (amplitude * exponent * sine);
    }

    public static Vector3 BounceOut(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        float bounce = 0.7f;

        if (t < 1f / bounce)
        {
            return start + (end - start) * (7.5625f * t * t);
        }
        else if (t < 2f / bounce)
        {
            t -= 1.5f / bounce;
            return start + (end - start) * (7.5625f * t * t + 0.75f);
        }
        else if (t < 2.5f / bounce)
        {
            t -= 2.25f / bounce;
            return start + (end - start) * (7.5625f * t * t + 0.9375f);
        }
        else
        {
            t -= 2.625f / bounce;
            return start + (end - start) * (7.5625f * t * t + 0.984375f);
        }
    }

    public static Vector3 Slerp(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Slerp(start, end, t);
    }

    public static Vector3 Exponential(Vector3 start, Vector3 end, float t)
    {
        return new Vector3(
            Mathf.Lerp(start.x, end.x, Mathf.Pow(t, 2)),
            Mathf.Lerp(start.y, end.y, Mathf.Pow(t, 2)),
            Mathf.Lerp(start.z, end.z, Mathf.Pow(t, 2))
        );
    }

    public static Vector3 QuadraticCurve(Vector3 start, Vector3 middle, Vector3 end, float t)
    {
        Vector3 p0 = Vector3.Lerp(start, middle, t);
        Vector3 p1 = Vector3.Lerp(middle, end, t);
        return Vector3.Lerp(p0, p1, t);
    }

    public static Vector3 CubicCurve(Vector3 start, Vector3 middle, Vector3 middle2, Vector3 end, float t)
    {
        Vector3 p0 = QuadraticCurve(start, middle, middle2, t);
        Vector3 p1 = QuadraticCurve(middle, middle2, end, t);
        return Vector3.Lerp(p0, p1, t);
    }

    #endregion

    #region Floats
    public static float NativeLerp(float startValue, float endValue, float t)
    {
        t = Mathf.Clamp01(t);
        return startValue + (endValue - startValue) * t;
    }
    public static float Linear(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, t);
    }

    public static float Squared(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, t * t);
    }

    public static float Cube(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, t * t * t);
    }

    public static float SquareRoot(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, Mathf.Sqrt(t));
    }

    public static float SmoothStep(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));
    }

    public static float QuadraticEaseOut(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, 1f - Mathf.Pow(1f - t, 2));
    }

    public static float Parabola(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, 4f * t * (1f - t));
    }

    public static float Triangle(float start, float end, float t)
    {
        return Mathf.Lerp(start, end, Mathf.PingPong(t, 1f));
    }

    public static float ElasticOut(float start, float end, float t)
    {
        float amplitude = 1f;
        float period = 0.3f;
        float overshoot = 1.70158f;

        t = Mathf.Clamp01(t);
        float x = t - 1f;
        float exponent = Mathf.Exp(overshoot * x);
        float sine = Mathf.Sin(period * Mathf.PI * t);

        return start + (end - start) * (amplitude * exponent * sine);
    }

    public static float BounceOut(float start, float end, float t)
    {
        t = Mathf.Clamp01(t);
        float bounce = 0.7f;

        if (t < 1f / bounce)
        {
            return start + (end - start) * (7.5625f * t * t);
        }
        else if (t < 2f / bounce)
        {
            t -= 1.5f / bounce;
            return start + (end - start) * (7.5625f * t * t + 0.75f);
        }
        else if (t < 2.5f / bounce)
        {
            t -= 2.25f / bounce;
            return start + (end - start) * (7.5625f * t * t + 0.9375f);
        }
        else
        {
            t -= 2.625f / bounce;
            return start + (end - start) * (7.5625f * t * t + 0.984375f);
        }
    }

    public static float ExponentialV2(float start, float end, float power, float t)
    {
        return Mathf.Lerp(start, end, Mathf.Pow(t, power));
    }
    public static float Exponential(float start, float end, float power, float t)
    {
        return Mathf.Lerp(start, end, 1 - Mathf.Pow(t, power));
    }
    #endregion

    #region Integers
    public static int Linear(int start, int end, float t)
    {
        return Mathf.RoundToInt(Mathf.Lerp(start, end, t));
    }

    public static int Squared(int start, int end, float t)
    {
        return Mathf.RoundToInt(Mathf.Lerp(start, end, t * t));
    }

    public static int Cube(int start, int end, float t)
    {
        return Mathf.RoundToInt(Mathf.Lerp(start, end, t * t * t));
    }

    public static int SquareRoot(int start, int end, float t)
    {
        return Mathf.RoundToInt(Mathf.Lerp(start, end, Mathf.Sqrt(t)));
    }

    public static int SmoothStep(int start, int end, float t)
    {
        return Mathf.RoundToInt(Mathf.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t)));
    }

    public static int QuadraticEaseOut(int start, int end, float t)
    {
        return Mathf.RoundToInt(Mathf.Lerp(start, end, 1f - Mathf.Pow(1f - t, 2)));
    }

    public static int Parabola(int start, int end, float t)
    {
        return Mathf.RoundToInt(Mathf.Lerp(start, end, 4f * t * (1f - t)));
    }

    public static int Triangle(int start, int end, float t)
    {
        return Mathf.RoundToInt(Mathf.Lerp(start, end, Mathf.PingPong(t, 1f)));
    }

    public static int ElasticOut(int start, int end, float t)
    {
        float amplitude = 1f;
        float period = 0.3f;
        float overshoot = 1.70158f;

        t = Mathf.Clamp01(t);
        float x = t - 1f;
        float exponent = Mathf.Exp(overshoot * x);
        float sine = Mathf.Sin(period * Mathf.PI * t);

        return start + Mathf.RoundToInt((end - start) * (amplitude * exponent * sine));
    }

    public static int BounceOut(int start, int end, float t)
    {
        t = Mathf.Clamp01(t);
        float bounce = 0.7f;

        if (t < 1f / bounce)
        {
            return start + Mathf.RoundToInt((end - start) * (7.5625f * t * t));
        }
        else if (t < 2f / bounce)
        {
            t -= 1.5f / bounce;
            return start + Mathf.RoundToInt((end - start) * (7.5625f * t * t + 0.75f));
        }
        else if (t < 2.5f / bounce)
        {
            t -= 2.25f / bounce;
            return start + Mathf.RoundToInt((end - start) * (7.5625f * t * t + 0.9375f));
        }
        else
        {
            t -= 2.625f / bounce;
            return start + Mathf.RoundToInt((end - start) * (7.5625f * t * t + 0.984375f));
        }
    }

    public static int Exponential(int start, int end, float t)
    {
        return Mathf.RoundToInt(Mathf.Lerp(start, end, Mathf.Pow(t, 2)));
    }
    #endregion

    #endregion

    #region Lerp

    public static float LerpCurrentValue(float startTime, float currentValue, float targetValue, float transitionTime)
    {
        // Calculate the current time elapsed since the start
        float currentTime = Time.time - startTime;

        // Ensure that the current time doesn't exceed the transition time
        float t = Mathf.Clamp01(currentTime / transitionTime);

        // Interpolate between the current value and target value using Mathf.Lerp
        float lerpedValue = Mathf.Lerp(currentValue, targetValue, t);

        return lerpedValue;
    }


    public static Quaternion LerpCurrentQuaternion(float startTime, Quaternion currentQuaternion, Quaternion targetQuaternion, float transitionTime)
    {
        // Calculate the current time elapsed since the start
        float currentTime = Time.time - startTime;

        // Ensure that the current time doesn't exceed the transition time
        float t = Mathf.Clamp01(currentTime / transitionTime);

        // Interpolate between the current quaternion and target quaternion using Quaternion.Lerp
        Quaternion lerpedQuaternion = Quaternion.Lerp(currentQuaternion, targetQuaternion, t);

        return lerpedQuaternion;
    }

    public static Vector3 LerpCurrentVector3(float startTime, Vector3 currentValue, Vector3 targetValue, float transitionTime)
    {
        // Calculate the current time elapsed since the start
        float currentTime = Time.time - startTime;

        // Ensure that the current time doesn't exceed the transition time
        float t = Mathf.Clamp01(currentTime / transitionTime);

        // Interpolate between the current vector and target vector using Vector3.Lerp
        Vector3 lerpedVector = Vector3.Lerp(currentValue, targetValue, t);

        return lerpedVector;
    }
    public static Vector3 LerpCurrentVector3(float startTime, Vector3 currentValue, float TargetValueX, float TargetValueY, float TargetValueZ, float transitionTime)
    {
        // Calculate the current time elapsed since the start
        float currentTime = Time.time - startTime;

        // Ensure that the current time doesn't exceed the transition time
        float t = Mathf.Clamp01(currentTime / transitionTime);

        // Interpolate between the current vector and target vector using Vector3.Lerp
        Vector3 lerpedVector = new Vector3(Mathf.Lerp(currentValue.x, TargetValueX, t), Mathf.Lerp(currentValue.y, TargetValueY, t), Mathf.Lerp(currentValue.z, TargetValueZ, t));

        return lerpedVector;
    }

    public static Vector2 LerpCurrentVector2(float startTime, Vector2 currentValue, Vector2 targetValue, float transitionTime)
    {
        // Calculate the current time elapsed since the start
        float currentTime = Time.time - startTime;

        // Ensure that the current time doesn't exceed the transition time
        float t = Mathf.Clamp01(currentTime / transitionTime);

        // Interpolate between the current vector and target vector using Vector2.Lerp
        Vector2 lerpedVector = Vector2.Lerp(currentValue, targetValue, t);

        return lerpedVector;
    }
    public static Vector2 LerpCurrentVector2(float startTime, Vector2 currentValue, float TargetValueX, float TargetValueY, float transitionTime)
    {
        // Calculate the current time elapsed since the start
        float currentTime = Time.time - startTime;

        // Ensure that the current time doesn't exceed the transition time
        float t = Mathf.Clamp01(currentTime / transitionTime);// *Vector2.Distance(currentValue,new Vector2(TargetValueX,TargetValueY));

        // Interpolate between the current vector and target vector using Vector2.Lerp
        Vector2 lerpedVector = new Vector2(Mathf.Lerp(currentValue.x, TargetValueX, t), Mathf.Lerp(currentValue.y, TargetValueY, t));

        return lerpedVector;
    }

    #endregion

    #region Int

    public static int IntWrapAround(int value, int min, int max)
    {
        int range = max - min + 1; // Calculate the range, including both min and max values
        value = ((value - min) % range + range) % range; // Apply modular arithmetic to wrap the value

        return value + min; // Add the min value back to obtain the wrapped value within the range
    }

    public static int RoundDownToNearestMultiple(int number, int multiple)
    {
        if (multiple == 0)
            return number;

        int remainder = number % multiple;
        int result = number - remainder;

        return result;
    }

    public static int RoundUpToNearestMultiple(int number, int multiple)
    {
        if (multiple == 0)
            return number;

        int remainder = number % multiple;
        int result = number + (multiple - remainder) % multiple;

        return result;
    }

    public static int RoundUpToNearestOddNumber(int number)
    {
        if (number % 2 == 0)
        {
            return number + 1; // Even number, add 1 to round up to the nearest odd number
        }
        else
        {
            return number; // Already an odd number, no need to round up
        }
    }

    public static int RoundDownToNearestOddNumber(int number)
    {
        if (number % 2 == 0)
        {
            return number - 1; // Even number, subtract 1 to round down to the nearest odd number
        }
        else
        {
            return number; // Already an odd number, no need to round down
        }
    }
    #endregion

    #region Float
    public static float RoundToDecimals(float value, int decimalPlaces)
    {
        float multiplier = Mathf.Pow(10f, decimalPlaces);
        return Mathf.Round(value * multiplier) / multiplier;
    }

    public static float FloorToDecimals(float value, int decimalPlaces)
    {
        float multiplier = Mathf.Pow(10f, decimalPlaces);
        return Mathf.Floor(value * multiplier) / multiplier;
    }
    public static float CeilingToDecimals(float value, int decimalPlaces)
    {
        float multiplier = Mathf.Pow(10f, decimalPlaces);
        return Mathf.Ceil(value * multiplier) / multiplier;
    }

    public static float WrapAround(float value, float min, float max)
    {
        float range = max - min; // Calculate the range
        float wrappedValue = (value - min) % range; // Apply modular arithmetic to wrap the value

        if (wrappedValue < 0) // Handle negative wrapped values
            wrappedValue += range;

        return wrappedValue + min; // Add the min value back to obtain the wrapped value within the range
    }

    public static float CalculatePercentage(float currentValue, params float[] values)
    {
        float maxValue = currentValue;
        foreach (float value in values)
        {
            maxValue += value;
        }

        if (maxValue == 0)
        {

            return 0.0f;
        }

        float percentage = (currentValue / maxValue) * 100.0f;
        return percentage;
    }
    #endregion

    #region Vector3

    public static Vector3 OrientationToVector3(Orientation orientation)
    {
        switch (orientation)
        {
            case Orientation.X:
                return Vector3.right;
            case Orientation.Y:
                return Vector3.up;
            case Orientation.Z:
                return Vector3.forward;
            case Orientation.N_X:
                return Vector3.left;
            case Orientation.N_Y:
                return Vector3.down;
            case Orientation.N_Z:
                return Vector3.back;
            default:
                return Vector3.zero;
        }
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 fromDirection, Vector3 toDirection)
    {
        // Calculate the initial rotation from the 'fromDirection' vector
        Quaternion fromRotation = Quaternion.LookRotation(fromDirection);

        // Calculate the target rotation towards the 'toDirection' vector
        Quaternion toRotation = Quaternion.LookRotation(toDirection);

        // Calculate the rotation delta
        Quaternion rotationDelta = toRotation * Quaternion.Inverse(fromRotation);

        // Convert the point and pivot to local coordinates
        Vector3 localPoint = point - pivot;

        // Apply the rotation to the local point
        Vector3 rotatedPoint = rotationDelta * localPoint;

        // Convert the rotated point back to world coordinates
        Vector3 finalPoint = rotatedPoint + pivot;

        return finalPoint;
    }
    public static Vector3 RotateVectorByDirections(Vector3 pos, Vector3 d1, Vector3 d2)
    {
        Vector3 normalizedDirectionA = d1.normalized;
        Vector3 normalizedDirectionB = d2.normalized;

        // Create a quaternion that rotates from directionA to directionB
        Quaternion rotation = Quaternion.FromToRotation(normalizedDirectionA, normalizedDirectionB);

        // Rotate the input vector using the rotation quaternion
        return rotation * pos;

    }
    public static Vector3 GetInterpolatedPosition(List<Vector3> positions, float totalDistance, float targetDistance)
    {
        if (positions == null || positions.Count == 0)
        {
            // Debug.LogError("Positions list is null or empty.");
            return Vector3.zero;
        }

        if (targetDistance <= 0)
        {
            return positions[0]; // Return the first position if the target distance is zero or negative
        }

        float accumulatedDistance = 0;
        int startIndex = 0;
        for (int i = 0; i < positions.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(positions[i], positions[i + 1]);

            if (accumulatedDistance + segmentDistance >= targetDistance)
            {
                startIndex = i;
                break;
            }
            accumulatedDistance += segmentDistance;
        }

        float remainingDistance = targetDistance - accumulatedDistance;
        Vector3 start = positions[startIndex];
        Vector3 end = positions[startIndex + 1];
        float interpolationRatio = remainingDistance / Vector3.Distance(start, end);

        return Vector3.Lerp(start, end, interpolationRatio);
    }
    public static Vector3 GetInterpolatedPositionUnclamped(List<Vector3> positions, float totalDistance, float targetDistance)
    {
        if (positions == null || positions.Count == 0)
        {
            // Debug.LogError("Positions list is null or empty.");
            return Vector3.zero;
        }


        float accumulatedDistance = 0;
        int startIndex = 0;
        bool reverseInterpolation = false;

        if (targetDistance < 0)
        {
            reverseInterpolation = true;
            targetDistance = Mathf.Abs(targetDistance);
        }
        if (targetDistance > totalDistance)
        {
            float remainingDistance2 = targetDistance - totalDistance;
            Vector3 start2 = positions[positions.Count - 2];
            Vector3 end2 = positions[positions.Count - 1];
            float interpolationRatio2 = remainingDistance2 / Vector3.Distance(start2, end2);
            Vector3 direction2 = (end2 - start2).normalized;
            return end2 + direction2 * (interpolationRatio2 * Vector3.Distance(start2, end2));
        }
        for (int i = 0; i < positions.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(positions[i], positions[i + 1]);

            if (reverseInterpolation)
            {
                if (accumulatedDistance - segmentDistance <= targetDistance)
                {
                    startIndex = i;
                    break;
                }
                accumulatedDistance -= segmentDistance;
            }
            else
            {

                if (((accumulatedDistance + segmentDistance) >= targetDistance) && !(i >= positions.Count - 1))
                {
                    startIndex = i;
                    break;
                }

                accumulatedDistance += segmentDistance;
            }
        }

        float remainingDistance = targetDistance - accumulatedDistance;
        Vector3 start = positions[startIndex];
        Vector3 end = positions[startIndex + 1];
        float interpolationRatio = remainingDistance / Vector3.Distance(start, end);

        if (reverseInterpolation)
        {
            Vector3 direction = (start - end).normalized;
            return start + direction * (interpolationRatio * Vector3.Distance(start, end));
        }
        else
        {
            return Vector3.Lerp(start, end, interpolationRatio);
        }
    }
    public static Vector3 GetInterpolatedPositionUnclampedV2(List<Vector3> positions, float totalDistance, float targetDistance)
    {
        if (positions == null || positions.Count == 0)
        {
            // Debug.LogError("Positions list is null or empty.");
            return Vector3.zero;
        }


        float accumulatedDistance = 0;
        int startIndex = 0;
        bool reverseInterpolation = false;

        if (targetDistance < 0)
        {
            reverseInterpolation = true;
            targetDistance = Mathf.Abs(targetDistance);
        }
        if (targetDistance > totalDistance)
        {
            float remainingDistance2 = targetDistance - totalDistance;
            Vector3 start2 = positions[positions.Count - 2];
            Vector3 end2 = positions[positions.Count - 1];
            float interpolationRatio2 = remainingDistance2 / Vector3.Distance(start2, end2);
            Vector3 direction2 = (end2 - start2).normalized;
            return end2 + direction2 * (interpolationRatio2 * Vector3.Distance(start2, end2));
        }
        for (int i = 0; i < positions.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(positions[i], positions[i + 1]);

            if (reverseInterpolation)
            {
                if (accumulatedDistance - segmentDistance <= targetDistance)
                {
                    startIndex = i;
                    break;
                }
                accumulatedDistance -= segmentDistance;
            }
            else
            {

                if (((accumulatedDistance + segmentDistance) >= targetDistance) && !(i >= positions.Count - 1))
                {
                    startIndex = i;
                    break;
                }

                accumulatedDistance += segmentDistance;
            }
        }

        float remainingDistance = targetDistance - accumulatedDistance;
        //(( positions[startIndex+1]-positions[startIndex])/2 + positions[startIndex])
        //Vector3 start = Vector3.Lerp(positions[startIndex ], positions[startIndex + 1],0.5f) ;
        //Vector3 end = Vector3.Lerp(positions[startIndex+1], positions[startIndex + 2], 0.5f);
        Vector3 start = positions[startIndex];
        Vector3 end = positions[startIndex + 1];

        float interpolationRatio = remainingDistance / Vector3.Distance(start, end);

        if (reverseInterpolation)
        {
            Vector3 direction = (start - end).normalized;
            return start + direction * (interpolationRatio * Vector3.Distance(start, end));
        }
        else
        {
            return Vector3.Lerp(start, end, interpolationRatio);
        }
    }
    public static Vector3 GetInterpolatedPositionDirection(List<Vector3> positions, float totalDistance, float targetDistance)
    {
        if (positions == null || positions.Count == 0)
        {
            // Debug.LogError("Positions list is null or empty.");
            return Vector3.zero;
        }

        if (targetDistance <= 0)
        {
            return GetDirection(positions[0], positions[1]); // Return the first position if the target distance is zero or negative
        }

        float accumulatedDistance = 0;
        int startIndex = 0;
        for (int i = 0; i < positions.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(positions[i], positions[i + 1]);

            if (accumulatedDistance + segmentDistance >= targetDistance)
            {
                startIndex = i;
                break;
            }
            accumulatedDistance += segmentDistance;
        }

        Vector3 start = positions[startIndex];
        Vector3 end = positions[startIndex + 1];

        return GetDirection(start, end);
    }
    public static int GetInterpolatedPositionIndex(List<Vector3> positions, float totalDistance, float targetDistance)
    {
        if (positions == null || positions.Count == 0)
        {
            // Debug.LogError("Positions list is null or empty.");
            return 0;
        }

        if (targetDistance <= 0)
        {
            return 0;
        }
        if (targetDistance > totalDistance)
        {
            return positions.Count - 1;
        }

        float accumulatedDistance = 0;
        for (int i = 0; i < positions.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(positions[i], positions[i + 1]);

            if (accumulatedDistance + segmentDistance >= targetDistance)
            {
                return i;

            }
            accumulatedDistance += segmentDistance;

        }
        if (accumulatedDistance < targetDistance)
            return positions.Count - 1;

        return 0;
    }
    public static int GetInterpolatedPositionIndexV2(List<Vector3> positions, float totalDistance, float targetDistance)
    {
        if (positions == null || positions.Count == 0)
        {
            // Debug.LogError("Positions list is null or empty.");
            return 0;
        }

        if (targetDistance <= 0)
        {
            return 0;
        }
        if (targetDistance > totalDistance)
        {
            return positions.Count - 2;
        }

        float accumulatedDistance = 0;
        for (int i = 0; i < positions.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(positions[i], positions[i + 1]);
            Debug.Log((accumulatedDistance + segmentDistance) + "//" + ((accumulatedDistance + segmentDistance) + ((accumulatedDistance + segmentDistance) / 2)));

            if (((accumulatedDistance + segmentDistance) + ((accumulatedDistance + segmentDistance) / 2)) >= targetDistance)
            {
                return i;

            }
            accumulatedDistance += segmentDistance;

        }

        return 0;
    }

    public static float GetInterpolatedPositionUnclampedValue(List<Vector3> positions, float totalDistance, float targetDistance)
    {
        if (positions == null || positions.Count == 0)
        {
            // Debug.LogError("Positions list is null or empty.");
            return 0f;
        }

        float accumulatedDistance = 0f;
        int startIndex = 0;
        bool reverseInterpolation = false;

        if (targetDistance < 0)
        {
            reverseInterpolation = true;
        }

        if (targetDistance > totalDistance)
        {
            float remainingDistance2 = targetDistance - totalDistance;
            Vector3 start2 = positions[positions.Count - 2];
            Vector3 end2 = positions[positions.Count - 1];
            float interpolationRatio2 = remainingDistance2 / Vector3.Distance(start2, end2);
            Vector3 direction2 = (end2 - start2).normalized;
            return end2.magnitude + direction2.magnitude * (interpolationRatio2 * Vector3.Distance(start2, end2));
        }

        for (int i = 0; i < positions.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(positions[i], positions[i + 1]);

            if (reverseInterpolation)
            {
                if (accumulatedDistance - segmentDistance <= targetDistance)
                {
                    startIndex = i;
                    break;
                }
                accumulatedDistance -= segmentDistance;
            }
            else
            {
                if (((accumulatedDistance + segmentDistance) >= targetDistance) && !(i >= positions.Count - 1))
                {
                    //    Debug.Log(accumulatedDistance + segmentDistance);
                    startIndex = i;
                    break;
                }

                accumulatedDistance += segmentDistance;
            }
        }

        float remainingDistance = targetDistance - accumulatedDistance;
        Vector3 start = positions[startIndex];
        Vector3 end = positions[startIndex + 1];
        float interpolationRatio = remainingDistance / Vector3.Distance(start, end);
        return interpolationRatio;

    }
    public static float GetInterpolatedPositionUnclampedValueV2(List<Vector3> positions, float totalDistance, float targetDistance)
    {
        if (positions == null || positions.Count == 0)
        {
            // Debug.LogError("Positions list is null or empty.");
            return 0f;
        }

        float accumulatedDistance = 0f;
        int startIndex = 0;
        bool reverseInterpolation = false;

        if (targetDistance < 0)
        {
            reverseInterpolation = true;
            return 0;
        }

        if (targetDistance > totalDistance)
        {
            float remainingDistance2 = targetDistance - totalDistance;
            Vector3 start2 = positions[positions.Count - 2];
            Vector3 end2 = positions[positions.Count - 1];
            float interpolationRatio2 = remainingDistance2 / Vector3.Distance(start2, end2);
            Vector3 direction2 = (end2 - start2).normalized;
            return end2.magnitude + direction2.magnitude * (interpolationRatio2 * Vector3.Distance(start2, end2));
        }

        for (int i = 0; i < positions.Count - 1; i++)
        {
            float segmentDistance = Vector3.Distance(positions[i], positions[i + 1]);

            if (reverseInterpolation)
            {
                if (accumulatedDistance - segmentDistance <= targetDistance)
                {
                    startIndex = i;
                    break;
                }
                accumulatedDistance -= segmentDistance;
            }
            else
            {
                if ((((accumulatedDistance + segmentDistance) + ((accumulatedDistance + segmentDistance) / 2)) >= targetDistance) && !(i >= positions.Count - 1))
                {
                    startIndex = i;
                    break;
                }

                accumulatedDistance += segmentDistance;
            }
        }

        float remainingDistance = targetDistance - accumulatedDistance;
        Vector3 start = positions[startIndex];
        Vector3 end = positions[startIndex + 1];
        float interpolationRatio = remainingDistance / Vector3.Distance(start, end);
        return interpolationRatio;
        if (reverseInterpolation)
        {
            Vector3 direction = (start - end).normalized;
            return start.magnitude + direction.magnitude * (interpolationRatio * Vector3.Distance(start, end));
        }
        else
        {
            // Calculate the endIndex based on the startIndex
            int endIndex = startIndex + 1;

            // Perform interpolation between start and end indices
            return Mathf.Lerp(start.magnitude, end.magnitude, interpolationRatio);
        }
    }

    public static Vector3 TransformPointWithForward(Vector3 position, Vector3 forward, Vector3 localPoint)
    {
        // Get the right and up vectors based on the forward direction
        Vector3 right = Vector3.Cross(forward, Vector3.up).normalized;
        Vector3 up = Vector3.Cross(right, forward).normalized;

        // Transform the local point to world space
        Vector3 worldPoint = position + localPoint.x * right + localPoint.y * up + localPoint.z * forward;

        return worldPoint;
    }
    public static Vector3 TransformPoint(Vector3 localPoint, Vector3 position, Vector3 forward, Vector3 up, Vector3 right)
    {


        // Transform the local point to world space
        Vector3 worldPoint = position + localPoint.x * right + localPoint.y * up + localPoint.z * forward;

        return worldPoint;
    }
    public static Vector3 RotateVectorByAngle(Vector3 vector, float angle, bool onX = false, bool onY = false, bool onZ = false)
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 rot = Vector3.zero;
        if (onX)
        {
            rot.x = angle;
        }
        if (onY)
        {
            rot.y = angle;

        }
        if (onZ)
        {
            rot.z = angle;

        }
        rotation = Quaternion.Euler(rot);
        return rotation * vector;
    }
    public static List<Vector3> EvaluatSlerpPoints(Vector3 start, Vector3 end, float centeroffset)
    {
        var centerpivot = (start + end) * .5f;
        centerpivot -= new Vector3(0, -centeroffset);
        var startrelativecenter = start - centerpivot;
        var endrelativecenter = end - centerpivot;

        var f = 1f / 10;
        List<Vector3> points = new List<Vector3>();
        for (float i = f; i < 1 + f; i += f)
        {
            points.Add(Vector3.Slerp(startrelativecenter, endrelativecenter, i) + centerpivot);
        }
        return points;
    }
    public static Vector3[] EvaluatSlerpPointsV2(Vector3 startPos, Vector3 endPos, int divisions)
    {
        Vector3[] positions = new Vector3[divisions + 1];

        for (int i = 0; i <= divisions; i++)
        {
            float t = (float)i / divisions;

            positions[i] = Vector3.Slerp(startPos, endPos, t);
        }

        return positions;
    }
    public static Vector3 AddVectors(bool allownonzero = true, params Vector3[] vectors)
    {
        Vector3 added = Vector3.zero;
        foreach (var item in vectors)
        {
            if (!allownonzero && item == Vector3.zero)
                continue;
            added += item;
        }
        return added;
    }
    public static Vector3 AverageVector(bool allownonzero = true, params Vector3[] vectors)
    {
        Vector3 added = Vector3.zero;
        int num = 0;
        foreach (var item in vectors)
        {
            if (!allownonzero && item == Vector3.zero)
                continue;
            added += item;
            num++;
        }
        // Debug.Log(num);
        return added / num;
    }

    public static int FindClosestPositionIndex(Vector3 targetPosition, params Vector3[] positions)
    {
        if (positions == null || positions.Length == 0)
        {
            Debug.LogError("No positions provided.");
            return -1;
        }

        int closestIndex = 0;
        float closestDistance = Vector3.Distance(targetPosition, positions[0]);

        for (int i = 1; i < positions.Length; i++)
        {
            float distance = Vector3.Distance(targetPosition, positions[i]);

            if (distance < closestDistance)
            {
                closestIndex = i;
                closestDistance = distance;
            }
        }

        return closestIndex;
    }
    public static Vector3 ExtendDirection(Vector3 direction, float distance)
    {
        Vector3 normalizedDirection = direction.normalized;

        Vector3 extendedDirection = direction + (normalizedDirection * distance);

        return extendedDirection;
    }
    public static Vector3 DivideVectors(Vector3 dividend, Vector3 divisor)
    {
        return new Vector3(
            dividend.x / divisor.x,
            dividend.y / divisor.y,
            dividend.z / divisor.z
        );
    }

    public static void CreatePositions(out Vector3[] positions, int Size)
    {
        // Create the array.
        positions = new Vector3[Size * Size * Size];

        // Iterate over the array and create the positions.
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                for (int k = 0; k < Size; k++)
                {
                    // Create the position.
                    positions[i * Size * Size + j * Size + k] = new Vector3(i, j, k);
                }
            }
        }
    }

    public static void CreatePositions(out Vector3[] positions, int xSize, int ySize, int zSize)
    {
        // Create the array.
        positions = new Vector3[xSize * ySize * zSize];

        // Iterate over the array and create the positions.
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                for (int k = 0; k < zSize; k++)
                {
                    // Create the position.
                    positions[i * ySize * zSize + j * zSize + k] = new Vector3(i, j, k);
                }
            }
        }
    }

    public static void CreateCircularPositions(out Vector3[] positions, Vector3 center, float radius, float distance)
    {
        // Create the array.
        positions = new Vector3[Mathf.FloorToInt(Mathf.Ceil((radius * 2) / distance))];

        // Iterate over the array and create the positions.
        for (int i = 0; i < positions.Length; i++)
        {
            // Create the position.
            positions[i] = new Vector3(
                center.x + radius * Mathf.Cos(2 * Mathf.PI * i / positions.Length),
                center.y + radius * Mathf.Sin(2 * Mathf.PI * i / positions.Length),
                center.z);
        }
    }


    public static Vector3[,] SortBounds(Vector3[] unsorted)
    {
        Vector3[] colors = new Vector3[] { unsorted[0], unsorted[1], unsorted[3], unsorted[2] };
        Vector3[,] sorted = new Vector3[4, 2];

        // Store the vectors in the 2D array
        for (int i = 0; i < 4; i++)
        {
            sorted[i, 0] = colors[i];
            sorted[i, 1] = colors[(i + 1) % 4];
        }
        return sorted;
    }

    public static Vector3 GetNoise(Vector3 pos, float _noise = 0)
    {
        _noise = Mathf.Clamp01(_noise);
        var noise = Mathf.PerlinNoise(pos.x * _noise, pos.z * _noise);

        return new Vector3(noise, 0, noise);
    }

    public static Vector3 EvaluatePointsBox(int _unitWidth = 5, int _unitDepth = 5, bool _hollow = false, float _nthOffset = 0, float _noise = 0, float Spread = 1)
    {
        var middleOffset = new Vector3(_unitWidth * 0.5f, 0, _unitDepth * 0.5f);
        _noise = Mathf.Clamp01(_noise);
        for (var x = 0; x < _unitWidth; x++)
        {
            for (var z = 0; z < _unitDepth; z++)
            {
                if (_hollow && x != 0 && x != _unitWidth - 1 && z != 0 && z != _unitDepth - 1) continue;
                var pos = new Vector3(x + (z % 2 == 0 ? 0 : _nthOffset), 0, z);

                pos -= middleOffset;

                pos += GetNoise(pos);

                pos *= Spread;

                return pos;
            }
        }
        return Vector3.zero;
    }

    public static List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    public static List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount, int mode = 0)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector3 dir;
            switch (mode)
            {
                case 0:
                    dir = ApplyRotationToVectorXY(new Vector3(1, 0, 0), angle);

                    break;
                case 1:
                    dir = ApplyRotationToVectorXZ(new Vector3(1, 0, 0), angle);

                    break;
                case 2:
                    dir = ApplyRotationToVectorYZ(new Vector3(0, 0, 1), angle);

                    break;

                default:
                    dir = ApplyRotationToVectorXY(new Vector3(1, 0, 0), angle);

                    break;
            }
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);

        }
        return positionList;
    }

    public static Vector3 ApplyRotationToVectorXY(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

    public static Vector3 ApplyRotationToVectorXZ(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vec;
    }

    public static Vector3 ApplyRotationToVectorYZ(Vector3 vec, float angle)
    {
        return Quaternion.Euler(angle, 0, 0) * vec;
    }

    public static void FaceForwardDirection(Transform transform, Vector3 targetDirection)
    {
        Vector3 targetPosition = transform.position + targetDirection;
        transform.LookAt(targetPosition);
    }

    public static void FaceRightDirection(Transform transform, Vector3 targetDirection)
    {
        Vector3 forwardDirection = transform.forward;
        Vector3 rightDirection = Vector3.Cross(forwardDirection, Vector3.up);
        Vector3 targetDirectionLocal = Quaternion.LookRotation(forwardDirection, Vector3.up) * targetDirection;
        Quaternion targetRotation = Quaternion.FromToRotation(rightDirection, targetDirectionLocal) * transform.rotation;
        Debug.Log(targetRotation);
        transform.rotation = targetRotation;
    }

    public static Vector3 RoundToNearestGridUnit(Vector3 position, float gridSize)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        //float y = Mathf.Round(position.y / gridSize) * gridSize;
        float z = Mathf.Round(position.z / gridSize) * gridSize;
        return new Vector3(x, position.y, z);
    }


    public static Vector3 EvaluatePointsCircle(int _amount = 10, float _radius = 1, float _radiusGrowthMultiplier = 0, float _rotations = 1, int _rings = 1, float _ringOffset = 1, float _nthOffset2 = 0, float Spread = 1)
    {
        var amountPerRing = _amount / _rings;
        var ringOffset = 0f;
        for (var i = 0; i < _rings; i++)
        {
            for (var j = 0; j < amountPerRing; j++)
            {
                var angle = j * Mathf.PI * (2 * _rotations) / amountPerRing + (i % 2 != 0 ? _nthOffset2 : 0);

                var radius = _radius + ringOffset + j * _radiusGrowthMultiplier;
                var x = Mathf.Cos(angle) * radius;
                var z = Mathf.Sin(angle) * radius;

                var pos = new Vector3(x, 0, z);

                pos += GetNoise(pos);

                pos *= Spread;

                return pos;
            }

            ringOffset += _ringOffset;
        }
        return Vector3.zero;
    }

    public static Vector3[] GeneratePath(Vector3 currentPosition, Vector3 finalPosition, int count)
    {
        Vector3[] path = new Vector3[count];

        Vector3 direction = (finalPosition - currentPosition) / (count - 1);

        for (int i = 0; i < count; i++)
        {
            path[i] = currentPosition + direction * i;
        }

        return path;
    }

    public static Vector3[] IteratePositions(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 direction = (endPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, endPosition);
        int steps = Mathf.CeilToInt(distance / 0.2f);

        Vector3[] positions = new Vector3[steps + 1];

        for (int i = 0; i <= steps; i++)
        {
            positions[i] = startPosition + direction * (i * 0.2f);
        }

        return positions;
    }

    public static Vector3 ClampDirection(Vector3 direction)
    {
        // Set the Y component to zero to restrict vertical movement
        direction.y = 0f;

        // Determine the dominant direction (left/right or forward/backward)
        bool isHorizontalDominant = Mathf.Abs(direction.x) >= Mathf.Abs(direction.z);

        // Clamp the non-dominant direction to zero
        if (isHorizontalDominant)
        {
            direction.z = 0f;
        }
        else
        {
            direction.x = 0f;
        }

        // Normalize the direction vector to ensure consistent movement speed
        direction.Normalize();

        return direction;
    }
    public static Vector3 UnDiagonalDirection(Vector3 start, Vector3 target)
    {
        // Set the Y component to zero to restrict vertical movement
        Vector3 direction = target - start;
        direction.y = 0f;

        // Determine the dominant direction (left/right or forward/backward)
        bool isHorizontalDominant = Mathf.Abs(direction.x) >= Mathf.Abs(direction.z);

        // Clamp the non-dominant direction to zero
        if (!isHorizontalDominant)
        {
            direction = new Vector3(start.x, 0, target.z);

        }
        else
        {
            direction = new Vector3(target.x, 0, start.z);

        }
        if (!CheckIfInNavMesh(direction))
        {
            if (!isHorizontalDominant)
            {
                direction = new Vector3(target.x, 0, start.z);


            }
            else
            {
                direction = new Vector3(start.x, 0, target.z);

            }
        }

        // Normalize the direction vector to ensure consistent movement speed
        return direction;
    }

    public static Vector3 MoveDirectionalIndpendent(Transform from, Vector3 movedirection)
    {
        float angle = Vector3.Angle(from.forward, movedirection);
        bool right = false;
        Vector3 cross = Vector3.Cross(from.forward, movedirection);
        if (cross.y < 0) right = false;
        if (cross.y > 0) right = true;
        if (movedirection.magnitude >= 0.1f)
        {


            if (angle <= 22.5f)
            {
                return Vector3.forward;

            }
            else if (angle <= 67.5 && angle > 22.5f)
            {
                if (right)
                {
                    return Vector3.forward + Vector3.right;

                }
                else if (right == false)
                {
                    return Vector3.forward + Vector3.left;

                }
            }
            else if (angle <= 112.5 && angle > 67.5f)
            {
                if (right)
                {
                    return Vector3.right;

                }
                else if (right == false)
                {
                    return Vector3.left;

                }
            }
            else if (angle <= 157.5 && angle > 112.5f)
            {
                if (right)
                {
                    return Vector3.back + Vector3.right;

                }
                else if (right == false)
                {
                    return Vector3.back + Vector3.left;

                }
            }
            else if (angle > 157.5)
            {
                return Vector3.back;



            }



        }

        return Vector3.zero;


    }

    public static Vector3 GetDirection(Vector3 origin, Vector3 target, bool flatX = false, bool flatY = false, bool flatZ = false, bool normalize = true, bool DominantTarget = false)
    {
        if (flatX)
        {
            if (DominantTarget)
                origin.x = target.x;
            else
                target.x = origin.x;
        }
        if (flatY)
        {
            if (DominantTarget)
                origin.y = target.y;
            else
                target.y = origin.y;
        }
        if (flatZ)
        {
            if (DominantTarget)
                origin.z = target.z;
            else
                target.z = origin.z;
        }
        Vector3 direction = GetDirectionNormal(origin, target, normalize);

        return direction;
    }

    public static Vector3 GetDirectionNormal(Vector3 origin, Vector3 target, bool normalize = true)
    {
        // Calculate the direction vector from the origin to the target
        Vector3 direction = target - origin;
        if (normalize)
            // Normalize the direction vector to have a magnitude of 1
            direction.Normalize();

        return direction;
    }

    public static bool IsDirectionDiagonal(Transform from, Vector3 movedirection)
    {
        float angle = Vector3.Angle(from.forward, movedirection);
        bool right = false;
        Vector3 cross = Vector3.Cross(from.forward, movedirection);
        if (cross.y < 0) right = false;
        if (cross.y > 0) right = true;


        if (movedirection.magnitude >= 0.1f)
        {


            if (angle <= 22.5f)
            {
                return false;
            }
            else if (angle <= 67.5 && angle > 22.5f)
            {
                if (right)
                {
                    return true;

                }
                else if (right == false)
                {
                    return true;

                }
            }
            else if (angle <= 112.5 && angle > 67.5f)
            {
                if (right)
                {
                    return false;

                }
                else if (right == false)
                {
                    return false;

                }
            }
            else if (angle <= 157.5 && angle > 112.5f)
            {
                if (right)
                {
                    return true;

                }
                else if (right == false)
                {
                    return true;

                }
            }
            else if (angle > 157.5)
            {
                return false;



            }



        }

        return false;


    }

    public static Vector3 GetDirectionCardinalDiagonal(Transform from, Vector3 movedirection)
    {
        float angle = Vector3.Angle(Vector3.forward + from.position, movedirection);
        bool right = false;
        Vector3 cross = Vector3.Cross(Vector3.forward + from.position, movedirection);
        if (cross.y < 0) right = false;
        if (cross.y > 0) right = true;


        if (movedirection.magnitude >= 0.1f)
        {


            if (angle <= 22.5f)
            {
                return Vector3.forward;
            }

            else if (angle <= 112.5 && angle > 67.5f)
            {
                if (right)
                {
                    return -Vector3.right;

                }
                else if (right == false)
                {
                    return -Vector3.right;

                }
            }

            else if (angle > 157.5)
            {
                return -Vector3.forward;



            }



        }

        return Vector3.zero;


    }

    public static Orientation RoundToNearestCardinalDirectionOrientation(Vector3 vector)
    {
        Vector3 right = Vector3.right;
        Vector3 left = Vector3.left;
        Vector3 forward = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 up = Vector3.up;
        Vector3 down = -Vector3.up;

        float dotRight = Vector3.Dot(vector, right);
        float dotLeft = Vector3.Dot(vector, left);
        float dotForward = Vector3.Dot(vector, forward);
        float dotBack = Vector3.Dot(vector, back);
        float dotDown = Vector3.Dot(vector, down);
        float dotUp = Vector3.Dot(vector, up);

        float maxDot = Mathf.Max(dotRight, dotLeft, dotForward, dotBack, dotDown, dotUp);

        if (dotRight == maxDot)
            return Orientation.X;
        else if (dotLeft == maxDot)
            return Orientation.N_X;
        else if (dotForward == maxDot)
            return Orientation.Z;
        else if (dotUp == maxDot)
            return Orientation.Y;
        else if (dotDown == maxDot)
            return Orientation.N_Y;

        else
            return Orientation.N_Z;

    }

    public static Vector3 RoundToNearestCardinalDirection(Vector3 vector)
    {
        Vector3 right = Vector3.right;
        Vector3 left = Vector3.left;
        Vector3 forward = Vector3.forward;
        Vector3 back = Vector3.back;
        Vector3 up = Vector3.up;
        Vector3 down = -Vector3.up;

        float dotRight = Vector3.Dot(vector, right);
        float dotLeft = Vector3.Dot(vector, left);
        float dotForward = Vector3.Dot(vector, forward);
        float dotBack = Vector3.Dot(vector, back);
        float dotDown = Vector3.Dot(vector, down);
        float dotUp = Vector3.Dot(vector, up);

        float maxDot = Mathf.Max(dotRight, dotLeft, dotForward, dotBack, dotDown, dotUp);

        if (dotRight == maxDot)
            return right;
        else if (dotLeft == maxDot)
            return left;
        else if (dotForward == maxDot)
            return forward;
        else if (dotUp == maxDot)
            return up;
        else if (dotDown == maxDot)
            return down;

        else
            return back;

    }

    public static Vector3 GetNearestDirectionCardinal(Transform from, Vector3 movedirection)
    {
        Vector3 right = from.right;
        Vector3 left = -from.right;
        Vector3 forward = from.forward;
        Vector3 back = -from.forward;
        Vector3 up = from.up;
        Vector3 down = -from.up;

        float dotRight = Vector3.Dot(movedirection, right);
        float dotLeft = Vector3.Dot(movedirection, left);
        float dotForward = Vector3.Dot(movedirection, forward);
        float dotBack = Vector3.Dot(movedirection, back);
        float dotDown = Vector3.Dot(movedirection, down);
        float dotUp = Vector3.Dot(movedirection, up);

        float maxDot = Mathf.Max(dotRight, dotLeft, dotForward, dotBack, dotDown, dotUp);

        if (dotRight == maxDot)
            return right;
        else if (dotLeft == maxDot)
            return left;
        else if (dotForward == maxDot)
            return forward;
        else if (dotUp == maxDot)
            return up;
        else if (dotDown == maxDot)
            return down;

        else
            return back;


    }
    public static Orientation GetNearestDirectionCardinalOrientation(Transform from, Vector3 movedirection)
    {
        Vector3 right = from.right;
        Vector3 left = -from.right;
        Vector3 forward = from.forward;
        Vector3 back = -from.forward;
        Vector3 up = from.up;
        Vector3 down = -from.up;

        float dotRight = Vector3.Dot(movedirection, right);
        float dotLeft = Vector3.Dot(movedirection, left);
        float dotForward = Vector3.Dot(movedirection, forward);
        float dotBack = Vector3.Dot(movedirection, back);
        float dotDown = Vector3.Dot(movedirection, down);
        float dotUp = Vector3.Dot(movedirection, up);

        float maxDot = Mathf.Max(dotRight, dotLeft, dotForward, dotBack, dotDown, dotUp);


        if (dotRight == maxDot)
            return Orientation.X;
        else if (dotLeft == maxDot)
            return Orientation.N_X;
        else if (dotForward == maxDot)
            return Orientation.Z;
        else if (dotUp == maxDot)
            return Orientation.Y;
        else if (dotDown == maxDot)
            return Orientation.N_Y;

        else
            return Orientation.N_Z;


    }

    public static Vector3 GetNextPoint(Vector3 start, Vector3 dir, float dist)
    {
        Vector3 displacement = dir.normalized * dist;
        Vector3 nextPoint = start + displacement;
        return nextPoint;
    }

    public static float GetAxisDirectionalSize(Vector3 input)
    {
        if (Mathf.Approximately(input.x, 1f) || Mathf.Approximately(input.x, -1f))
        {
            return input.x;
        }

        if (Mathf.Approximately(input.y, 1f) || Mathf.Approximately(input.y, -1f))
        {
            return input.y;
        }

        if (Mathf.Approximately(input.z, 1f) || Mathf.Approximately(input.z, -1f))
        {
            return input.z;
        }

        Debug.LogWarning("Input does not match any axis (X, Y, Z).");
        return 0f;
    }

    public static Vector3 ConvertToAbsolute(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public static float GetNonZeroComponent(Vector3 vector)
    {
        if (vector.x != 0)
        {
            return vector.x;
        }

        if (vector.y != 0)
        {
            return vector.y;
        }

        if (vector.z != 0)
        {
            return vector.z;
        }

        Debug.LogWarning("All components of the vector are zero.");
        return 0f;
    }

    public static Vector3 MoveVectorAlongNormalizedDirection(Vector3 startPoint, Vector3 direction, float distance)
    {
        // Normalize the direction vector
        Vector3 normalizedDirection = direction.normalized;

        // Calculate the displacement vector
        Vector3 displacement = normalizedDirection * distance;

        // Calculate the new position
        Vector3 newPosition = startPoint + displacement;

        return newPosition;
    }
    public static Vector3 MoveVectorAlongDirection(Vector3 origin, Vector3 direction)
    {
        Vector3 newPosition = origin + direction;
        return newPosition;
    }
    public static Vector3 FindClosestPoint(Vector3 origin, params Vector3[] points)
    {
        if (points.Length == 0)
        {
            Debug.LogError("No points provided.");
            return Vector3.zero;
        }

        Vector3 closestPoint = points[0];
        float closestDistance = Vector3.Distance(origin, closestPoint);

        for (int i = 1; i < points.Length; i++)
        {
            float distance = Vector3.Distance(origin, points[i]);
            if (distance < closestDistance)
            {
                closestPoint = points[i];
                closestDistance = distance;
            }
        }

        return closestPoint;
    }
    public static Vector3 FindClosestPointOnPlane(Vector3 targetVector, Vector3[] planepoints)
    {
        Vector3 planeNormal = Vector3.Cross(planepoints[1] - planepoints[0], planepoints[2] - planepoints[0]).normalized;

        float distance = Vector3.Dot(planeNormal, targetVector - planepoints[0]);
        Vector3 closestPointOnPlane = targetVector - distance * planeNormal;

        return closestPointOnPlane;
    }
    public static Vector3 FindClosestPointFromTwoOrigins(Vector3 origin1, Vector3 origin2, params Vector3[] points)
    {
        if (points.Length == 0)
        {
            //  Debug.LogError("No points provided.");
            return Vector3.zero;
        }

        Vector3 closestPoint = points[0];
        float closestDistance = Mathf.Min(Vector3.Distance(origin1, closestPoint), Vector3.Distance(origin2, closestPoint));

        for (int i = 1; i < points.Length; i++)
        {
            float distance1 = Vector3.Distance(origin1, points[i]);
            float distance2 = Vector3.Distance(origin2, points[i]);

            if (distance1 < closestDistance && distance1 < distance2)
            {
                closestPoint = points[i];
                closestDistance = distance1;
            }
            else if (distance2 < closestDistance)
            {
                closestPoint = points[i];
                closestDistance = distance2;
            }
        }

        return closestPoint;
    }

    public static Vector3 ScaleMoveRotateVector(Transform transform, Vector3 vector)
    {
        Vector3 scaledVector = Vector3.Scale(vector, transform.localScale);
        Vector3 rotatedVector = transform.rotation * scaledVector;
        Vector3 finalVector = rotatedVector + transform.position;
        return finalVector;
    }
    public static Vector3 ReverseScaleMoveRotateVector(Transform transform, Vector3 vector)
    {
        // Undo translation
        Vector3 untranslatedVector = vector - transform.position;

        // Undo rotation
        Quaternion inverseRotation = Quaternion.Inverse(transform.rotation);
        Vector3 unrotatedVector = inverseRotation * untranslatedVector;

        // Undo scaling
        Vector3 inverseScale = new Vector3(
            Mathf.Approximately(transform.localScale.x, 0) ? 0 : 1 / transform.localScale.x,
            Mathf.Approximately(transform.localScale.y, 0) ? 0 : 1 / transform.localScale.y,
            Mathf.Approximately(transform.localScale.z, 0) ? 0 : 1 / transform.localScale.z
        );
        Vector3 unscaledVector = Vector3.Scale(unrotatedVector, inverseScale);

        return unscaledVector;
    }
    public static bool CheckCollisionOnDirection(Vector3 vectorA, Vector3 vectorB)
    {
        // Check if the vectors overlap
        if (vectorA == vectorB)
        {
            return true;
        }

        // Check if the vectors intersect
        Vector3 direction = vectorB - vectorA;
        Ray ray = new Ray(vectorA, direction);
        float distance = direction.magnitude;

        if (Physics.Raycast(ray, distance))
        {
            return true;
        }

        return false;
    }

    public static Vector3 SpreadDirection(Vector3 origin, Vector3 direction, float minspread, float maxspread)
    {
        Vector3 directionwithoutspread = direction - origin;

        float x = UnityEngine.Random.Range(minspread, maxspread);
        float y = UnityEngine.Random.Range(minspread, maxspread);

        return directionwithoutspread + new Vector3(x, y, 0);
    }
    public static Vector3 RoundDirection(Vector3 forwardDirection, Vector3 targetDirection, float roundingIncrement)
    {
        // Calculate the signed angle between the forward direction and the target direction
        float signedAngle = Vector3.SignedAngle(forwardDirection, targetDirection, Vector3.up);

        // Shift the signed angle to be within the range of [0, 360)
        float shiftedAngle = (signedAngle + 360f) % 360f;

        // Round the shifted angle to the nearest roundingIncrement
        int roundedAngle = Mathf.RoundToInt(shiftedAngle / roundingIncrement) * Mathf.RoundToInt(roundingIncrement);

        // Calculate the rounded direction based on the rounded angle
        Quaternion rotation = Quaternion.Euler(0f, roundedAngle, 0f);
        Vector3 roundedDirection = rotation * forwardDirection;

        return roundedDirection;
    }
    public static void FindMinMax(Vector3[] vectors, out Vector3 min, out Vector3 max)
    {
        if (vectors == null || vectors.Length == 0)
        {
            Debug.LogError("Vector3 array is null or empty.");
            min = Vector3.zero;
            max = Vector3.zero;
            return;
        }

        min = vectors[0];
        max = vectors[0];

        for (int i = 1; i < vectors.Length; i++)
        {
            min.x = Mathf.Min(min.x, vectors[i].x);
            min.y = Mathf.Min(min.y, vectors[i].y);
            min.z = Mathf.Min(min.z, vectors[i].z);

            max.x = Mathf.Max(max.x, vectors[i].x);
            max.y = Mathf.Max(max.y, vectors[i].y);
            max.z = Mathf.Max(max.z, vectors[i].z);
        }
    }

    public static Vector3[] GetMinMaxCorners(Vector3 min, Vector3 max)
    {
        Vector3[] corners = new Vector3[8];

        // Front face
        corners[0] = new Vector3(min.x, min.y, min.z);
        corners[1] = new Vector3(max.x, min.y, min.z);
        corners[2] = new Vector3(max.x, max.y, min.z);
        corners[3] = new Vector3(min.x, max.y, min.z);

        // Back face
        corners[4] = new Vector3(min.x, min.y, max.z);
        corners[5] = new Vector3(max.x, min.y, max.z);
        corners[6] = new Vector3(max.x, max.y, max.z);
        corners[7] = new Vector3(min.x, max.y, max.z);

        return corners;
    }
    #endregion

    #region Quaternion
    public static Quaternion ForwardRotateBasedOnSurface(GameObject obj, bool isfrombottom, float maxraycastdistance)
    {
        Bounds bound = Utility.GetBounds(obj);
        Vector3 origin = obj.transform.position;
        if (isfrombottom)
            origin.y += bound.size.y / 2;
        Vector3 forwarded = (Vector3.Scale(bound.size, obj.transform.forward) / 2) + origin;
        Vector3 backwards = (Vector3.Scale(bound.size, -obj.transform.forward) / 2) + origin;

        Vector3 downwardf = Vector3.down + forwarded;
        Vector3 downwardb = Vector3.down + backwards;

        RaycastHit hitb;
        RaycastHit hitf;

        Physics.Raycast(forwarded, downwardf - forwarded, out hitf, maxraycastdistance);
        Physics.Raycast(backwards, downwardb - backwards, out hitb, maxraycastdistance);

        Vector3 upright = Vector3.Cross(obj.transform.right, -(hitf.point - hitb.point).normalized);

        return Quaternion.LookRotation(Vector3.Cross(obj.transform.right, upright));

    }

    public static Quaternion CardinalRotateBasedOnSurface(GameObject obj, bool isfrombottom, float maxraycastdistance)
    {
        Bounds bound = Utility.GetBounds(obj);
        Vector3 origin = obj.transform.position;
        if (isfrombottom)
            origin.y += bound.size.y / 2;
        Vector3 forwarded = (Vector3.Scale(bound.size, obj.transform.forward) / 2) + origin;
        Vector3 backwards = (Vector3.Scale(bound.size, -obj.transform.forward) / 2) + origin;
        Vector3 rightward = (Vector3.Scale(bound.size, obj.transform.right) / 2) + origin;
        Vector3 leftwards = (Vector3.Scale(bound.size, -obj.transform.right) / 2) + origin;

        Vector3 downward = Vector3.down + origin;
        Vector3 downwardf = Vector3.down + forwarded;
        Vector3 downwardb = Vector3.down + backwards;
        Vector3 downwardr = Vector3.down + rightward;
        Vector3 downwardl = Vector3.down + leftwards;



        RaycastHit hit;
        RaycastHit hitb;
        RaycastHit hitf;
        RaycastHit hitr;
        RaycastHit hitl;



        Physics.Raycast(origin, downward - origin, out hit, maxraycastdistance);
        Physics.Raycast(forwarded, downwardf - forwarded, out hitf, maxraycastdistance);
        Physics.Raycast(backwards, downwardb - backwards, out hitb, maxraycastdistance);
        Physics.Raycast(rightward, downwardr - rightward, out hitr, maxraycastdistance);
        Physics.Raycast(leftwards, downwardl - leftwards, out hitl, maxraycastdistance);



        Vector3 averageNormal = Utility.AverageVector(false, hit.normal + hitf.normal + hitb.normal + hitr.normal + hitl.normal);

        Quaternion targetRotation = Quaternion.FromToRotation(obj.transform.up, averageNormal) * obj.transform.rotation;

        return targetRotation;




    }

    public static Quaternion MultiDirectionalRotateBasedOnSurface(GameObject obj, bool isfrombottom, float maxraycastdistance)
    {
        Bounds bound = Utility.GetBounds(obj);
        Vector3 origin = obj.transform.position;
        if (isfrombottom)
            origin.y += bound.size.y / 2;
        Vector3 forwarded = (Vector3.Scale(bound.size, obj.transform.forward) / 2) + origin;
        Vector3 backwards = (Vector3.Scale(bound.size, -obj.transform.forward) / 2) + origin;
        Vector3 rightward = (Vector3.Scale(bound.size, obj.transform.right) / 2) + origin;
        Vector3 leftwards = (Vector3.Scale(bound.size, -obj.transform.right) / 2) + origin;

        Vector3 forwardedright = Vector3.Lerp(forwarded, rightward, 0.5f);
        Vector3 backwardright = Vector3.Lerp(backwards, rightward, 0.5f);
        Vector3 forwardedleft = Vector3.Lerp(forwarded, leftwards, 0.5f);
        Vector3 backwardleft = Vector3.Lerp(backwards, leftwards, 0.5f);

        Vector3 downward = Vector3.down + origin;
        Vector3 downwardf = Vector3.down + forwarded;
        Vector3 downwardb = Vector3.down + backwards;
        Vector3 downwardr = Vector3.down + rightward;
        Vector3 downwardl = Vector3.down + leftwards;

        Vector3 downwardfl = Vector3.down + forwardedleft;
        Vector3 downwardbl = Vector3.down + backwardleft;
        Vector3 downwardbr = Vector3.down + backwardright;
        Vector3 downwardfr = Vector3.down + forwardedright;


        RaycastHit hit;
        RaycastHit hitb;
        RaycastHit hitf;
        RaycastHit hitr;
        RaycastHit hitl;

        RaycastHit hitfr;
        RaycastHit hitfl;
        RaycastHit hitbr;
        RaycastHit hitbl;

        Physics.Raycast(origin, downward - origin, out hit, maxraycastdistance);
        Physics.Raycast(forwarded, downwardf - forwarded, out hitf, maxraycastdistance);
        Physics.Raycast(backwards, downwardb - backwards, out hitb, maxraycastdistance);
        Physics.Raycast(rightward, downwardr - rightward, out hitr, maxraycastdistance);
        Physics.Raycast(leftwards, downwardl - leftwards, out hitl, maxraycastdistance);

        Physics.Raycast(forwardedleft, downwardfl - forwardedleft, out hitfr, maxraycastdistance);
        Physics.Raycast(backwardleft, downwardbl - backwardleft, out hitbl, maxraycastdistance);
        Physics.Raycast(backwardright, downwardbr - backwardright, out hitbr, maxraycastdistance);
        Physics.Raycast(forwardedright, downwardfr - forwardedright, out hitfl, maxraycastdistance);




        // Apply the rotation to your object
        Vector3 averageNormal = Utility.AverageVector(false, hit.normal + hitf.normal + hitb.normal + hitr.normal + hitl.normal + hitfr.normal + hitbr.normal + hitbl.normal + hitfl.normal);

        // Calculate the target rotation based on the average normal
        Quaternion targetRotation = Quaternion.FromToRotation(obj.transform.up, averageNormal) * obj.transform.rotation;

        // Rotate the object towards the target rotation
        return targetRotation;


    }

    public static Quaternion LookAtDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        return targetRotation;
    }

    #endregion

    #region Bounds

    public static Vector3 GetGameObjectBoundsSize(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size;
        }

        Collider collider = gameObject.GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds.size;
        }

        Debug.LogWarning("No Renderer or Collider found on the GameObject.");
        return Vector3.zero;
    }
    public static Bounds GetBounds(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        Collider collider = obj.GetComponent<Collider>();

        if (renderer != null)
            return renderer.bounds;
        else if (collider != null)
            return collider.bounds;

        // If no renderer or collider found, use a default bounds
        Bounds bounds = new Bounds(obj.transform.position, Vector3.one);
        return bounds;
    }
    #endregion

    #region Image

    public static Texture2D ColorArrayToTexture2D(UnityEngine.Color[] colorArray, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(colorArray);
        texture.Apply();

        return texture;
    }

    public static byte[] StringToBytes(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        return bytes;
    }

    public static byte[] ImageToBytes(string imagePath)
    {
        // Load the image from the file path
        Texture2D texture = new Texture2D(2, 2); // Create a temporary Texture2D
        byte[] bytes;

        if (File.Exists(imagePath))
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);

            // Load the image data into the Texture2D
            if (texture.LoadImage(imageBytes))
            {
                // Convert the Texture2D to bytes
                bytes = texture.EncodeToPNG();
            }
            else
            {
                Debug.LogError("Failed to load image: " + imagePath);
                bytes = new byte[0];
            }
        }
        else
        {
            Debug.LogError("Image file not found: " + imagePath);
            bytes = new byte[0];
        }

        GameObject.Destroy(texture); // Destroy the temporary Texture2D to free up memory
        return bytes;
    }

    public static byte[] ImageToBytes(Texture2D image)
    {
        byte[] bytes = image.GetRawTextureData();
        return bytes;
    }

    public static Texture2D BytesToTexture2D(byte[] bytes)
    {
        //Texture2D tex = new Texture2D(32, 32, TextureFormat.DXT1, false);
        Texture2D tex = new Texture2D(32, 32, TextureFormat.DXT1, false);

        tex.LoadRawTextureData(bytes);
        tex.Apply();
        return tex;
    }
    #endregion

    #region Color

    public static UnityEngine.Color StringToColor(string colorString)
    {
        string[] colorValues = colorString.Replace("RGBA(", "").Replace(")", "").Split(',');

        if (colorValues.Length == 4)
        {
            float r, g, b, a;
            if (float.TryParse(colorValues[0], out r) && float.TryParse(colorValues[1], out g) &&
                float.TryParse(colorValues[2], out b) && float.TryParse(colorValues[3], out a))
            {
                return new UnityEngine.Color(r, g, b, a);
            }
        }

        // Return default color if the conversion fails
        return UnityEngine.Color.white;
    }

    public static UnityEngine.Color[] StringArrayToColorArray(string[] stringArray)
    {
        UnityEngine.Color[] colorArray = new UnityEngine.Color[stringArray.Length];

        for (int i = 0; i < stringArray.Length; i++)
        {

            colorArray[i] = StringToColor(stringArray[i]);
            Debug.Log(colorArray[i].ToString());
        }

        return colorArray;
    }

    public static UnityEngine.Color[] GetPixelColors(Texture2D texture)
    {
        UnityEngine.Color[] pixelColors = new UnityEngine.Color[texture.width * texture.height];

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                pixelColors[x + y * texture.width] = texture.GetPixel(x, y);
            }
        }

        return pixelColors;
    }

    public static string[] ColorArrayToString(UnityEngine.Color[] colors)
    {
        string[] colorDataArray = new string[colors.Length];

        for (int i = 0; i < colors.Length; i++)
        {
            colorDataArray[i] = colors[i].ToString();
            Debug.Log(colors[i].ToString());
        }

        return colorDataArray;
    }

    [System.Serializable]
    public class ColorData
    {
        public float r;
        public float g;
        public float b;
        public float a;
        public ColorData(UnityEngine.Color color)
        {
            r = color.r;
            g = color.g;
            b = color.b;
            a = color.a;
        }

        public UnityEngine.Color ToColor()
        {
            return new UnityEngine.Color(r, g, b, a);
        }
    }

    public static UnityEngine.Color[] StringToColorArray(string jsonString)
    {
        ColorData[] colorDataArray = JsonUtility.FromJson<ColorData[]>(jsonString);
        UnityEngine.Color[] colors = new UnityEngine.Color[colorDataArray.Length];

        for (int i = 0; i < colorDataArray.Length; i++)
        {
            colors[i] = colorDataArray[i].ToColor();
        }

        return colors;
    }
    #endregion

    #region String

    public static string StringArrayToString(string[] stringArray)
    {
        string joinedString = string.Join("/", stringArray);
        return joinedString;
    }

    public static string[] StringToStringArray(string str)
    {
        string[] stringArray = str.Split('/');
        return stringArray;
    }

    public static string[] ConvertToStringArray<T>(T[] originalArray)
    {
        string[] stringArray = new string[originalArray.Length];

        for (int i = 0; i < originalArray.Length; i++)
        {
            stringArray[i] = originalArray[i].ToString();
        }

        return stringArray;
    }
    #endregion

    #region Colliders
    public static bool CheckCollision(GameObject object1, GameObject object2)
    {
        Collider collider1 = object1.GetComponent<Collider>();
        Collider collider2 = object2.GetComponent<Collider>();

        if (collider1 != null && collider2 != null)
        {
            return collider1.bounds.Intersects(collider2.bounds);
        }

        // Return false if either object does not have a collider
        return false;
    }
    #endregion

    #region Object Rotation
    public static float RotateSecondaryWithConstraint(Transform body, Transform Secondary, Vector3 targetPosition, float Min_Rotation, float Max_Rotation, float Rotation_Speed, float rotation)
    {
        //secondary = head or turret
        //body = the forward angle body
        Vector3 turretUp = body.transform.up;
        Vector3 vecToTarget = targetPosition - Secondary.position;
        Vector3 flattenedVecForBase = Vector3.ProjectOnPlane(vecToTarget, turretUp);

        //limited
        if (Min_Rotation != 360)
        {
            Vector3 turretForward = body.transform.forward;
            float targetTraverse = Vector3.SignedAngle(turretForward, flattenedVecForBase, turretUp);

            targetTraverse = Mathf.Clamp(targetTraverse, -Min_Rotation, Max_Rotation);
            rotation = Mathf.MoveTowards(rotation, targetTraverse, Rotation_Speed * Time.deltaTime);

            if (Mathf.Abs(rotation) > Mathf.Epsilon)
                Secondary.localEulerAngles = Vector3.up * rotation;
        }
        else
        {
            Secondary.rotation = Quaternion.RotateTowards(
                Quaternion.LookRotation(Secondary.forward, turretUp),
                Quaternion.LookRotation(flattenedVecForBase, turretUp),
                Rotation_Speed * Time.deltaTime);
        }
        return rotation;
    }
    public static void RotateSecondaryWithoutConstraint(Transform body, Transform Secondary, Vector3 targetPosition, float Rotation_Speed)
    {
        Vector3 turretUp = body.transform.up;
        Vector3 vecToTarget = targetPosition - Secondary.position;
        Vector3 flattenedVecForBase = Vector3.ProjectOnPlane(vecToTarget, turretUp);


        Secondary.transform.rotation = Quaternion.RotateTowards(
            Quaternion.LookRotation(Secondary.transform.forward, turretUp),
            Quaternion.LookRotation(flattenedVecForBase, turretUp), Rotation_Speed * Time.deltaTime);


    }
    public static void SmoothTurnToDirection(Transform objects, Vector3 direction, float turnspeed)
    {

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        targetRotation.x = 0;
        targetRotation.z = 0;
        objects.transform.rotation = Quaternion.Lerp(objects.transform.rotation, targetRotation, turnspeed * Time.deltaTime);

    }
    public static void LinearTurnToDirection(Transform objects, Vector3 direction, float rotationSpeed)
    {
        Vector3 currentDirection = objects.transform.forward;
        Vector3 newDirection = Vector3.RotateTowards(currentDirection, direction, rotationSpeed * Time.deltaTime, 0f);
        Quaternion newRotation = Quaternion.LookRotation(newDirection);
        newRotation.x = 0;
        newRotation.z = 0;
        objects.transform.rotation = newRotation;
    }
    public static void RotateToTargetSmooth(Transform body, Vector3 target, float rotationSpeed)
    {
        Vector3 direction = target - body.transform.position;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Slerp between the current rotation and the target rotation
        Quaternion newRotation = Quaternion.Slerp(body.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Apply the new rotation to the object
        body.transform.rotation = newRotation;
    }
    public static void RotateToTargetLinear(Transform body, Vector3 target, float rotationSpeed)
    {
        // Calculate the direction to the target
        Vector3 direction = target - body.transform.position;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Rotate towards the target rotation at a constant speed
        body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
    public static float ElevateRotation(Transform body, Transform barrel, Vector3 targetPosition, float Min_Elevation, float Max_Elevation, float Elevation_Speed, float elevation)
    {
        Vector3 localTargetPos = body.InverseTransformDirection(targetPosition - barrel.position);
        Vector3 flattenedVecForBarrels = Vector3.ProjectOnPlane(localTargetPos, Vector3.up);

        float targetElevation = Vector3.Angle(flattenedVecForBarrels, localTargetPos);
        targetElevation *= Mathf.Sign(localTargetPos.y);

        targetElevation = Mathf.Clamp(targetElevation, -Min_Elevation, Max_Elevation);
        elevation = Mathf.MoveTowards(elevation, targetElevation, Elevation_Speed * Time.deltaTime);

        if (Mathf.Abs(elevation) > Mathf.Epsilon)
            barrel.localEulerAngles = Vector3.right * -elevation;
        return elevation;
    }

    #endregion

    #region Struct
    public static Squad FindFromListStruct(List<Squad> squads, string name)
    {
        return squads.Find(x => x.name == name);

    }

    public static void RemoveFromListStruct(List<Squad> squads, GameObject unit, string name)
    {
        Squad sq = FindFromListStruct(squads, name);
        if (!sq.Equals(default(Squad)))
        {
            sq.units.Remove(unit);
        }


    }

    public static bool CheckFromListStruct(List<Squad> squads, string name)
    {
        return squads.Any(x => x.name == name);
    }

    public static bool CheckSquadListStruct(List<Squad> squads, List<GameObject> units)
    {
        Squad squad = new Squad();
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].GetComponent<UnitAI>().SquadName != "")
            {
                squad = squads.FirstOrDefault(x => x.name == units[i].GetComponent<UnitAI>().SquadName);

            }

            if (!squad.Equals(default(Squad)))
            {
                if (squad.Equals(squads.FirstOrDefault(x => x.name == units[i].GetComponent<UnitAI>().SquadName)))
                {
                    Debug.Log("Same Squad");
                    return false;
                }
            }
        }
        return true;
    }

    public static Squad GetDefaultFromListStruct(List<Squad> squads, string name)
    {
        Squad squad = squads.FirstOrDefault(x => x.name == name);
        bool squadExists = !squad.Equals(default(Squad));
        if (squadExists)
            return squad;
        else
            return default(Squad);
    }
    #endregion

    #region Time
    public static float TimeSeconds()
    {
        float deltaTime = Time.deltaTime;
        float oneSecond = deltaTime / 1.0f;
        // Debug.Log("Number of frames per second: " + 1.0f / oneSecond);
        return oneSecond;
    }
    public static float TimeMinutes()
    {
        float deltaTime = Time.deltaTime;
        float oneMinute = deltaTime / 60.0f;
        // Debug.Log("Number of frames per minute: " + 1.0f / oneMinute);
        return oneMinute;
    }

    public static float TimeHour()
    {
        float deltaTime = Time.deltaTime;
        float oneHour = deltaTime / 3600.0f;
        // Debug.Log("Number of frames per hour: " + 1.0f / oneHour);
        return oneHour;
    }
    public static float TimeAdd(float value)
    {
        return Time.time + value;
    }
    public static float TimeAddClamp(float value)
    {
        return Time.time + Mathf.Clamp01(value);
    }

    public static void InterpolateInTime(float startTime, float endTime, float valuereferenceTime, float min, float max)
    {
        if (valuereferenceTime >= startTime && valuereferenceTime < endTime)
        {
            // Perform the interpolation
            float t = (valuereferenceTime - startTime);
            float interpolatedValue = Mathf.Lerp(min, max, t);

            // Do something with the interpolated value...
            Debug.Log("Interpolated value: " + interpolatedValue);
        }

    }

    public static bool CheckDuration(float startTime, float duration)
    {
        Debug.Log(Time.time + "//" + startTime + "/=/" + (Time.time - startTime) + "///" + duration);
        if (Time.time - startTime >= duration)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    #endregion

    #region Generics
    public static List<T> ConvertToArrayToList<T>(T[] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array), "Input array cannot be null.");
        }

        return new List<T>(array);
    }
    public static List<T> GetListWithoutIndex<T>(List<T> originalList, int index)
    {
        List<T> newList = new List<T>();

        for (int i = 0; i < originalList.Count; i++)
        {
            if (i != index)
            {
                newList.Add(originalList[i]);
            }
        }

        return newList;
    }
    public static List<T> ConvertToList<T>(T[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        List<T> list = new List<T>(array.Length);
        list.AddRange(array);
        return list;
    }

    public static T ConvertToType<T>(object input)
    {
        T convertedValue = (T)Convert.ChangeType(input, typeof(T));
        return convertedValue;
    }

    public static UnityEngine.Object ConvertToUnityObject<T>(T value) where T : class
    {
        UnityEngine.Object unityObject = value as UnityEngine.Object;
        // Or alternatively:
        // Object unityObject = (Object)value;

        return unityObject;
    }

    public static T[] ReverseArray<T>(T[] array)
    {
        T[] reversedArray = new T[array.Length];
        int currentIndex = array.Length - 1;

        for (int i = 0; i < array.Length; i++)
        {
            reversedArray[i] = array[currentIndex];
            currentIndex--;
        }

        return reversedArray;
    }

    public static bool HasVariable<T>(string variableName, T classname) where T : class
    {
        Type type = classname.GetType();
        FieldInfo fieldInfo = type.GetField(variableName, BindingFlags.Public | BindingFlags.Instance);

        return fieldInfo != null;
    }

    public static object GetVariableValue<T>(T obj, string variableName) where T : class
    {
        Type type = typeof(T);
        FieldInfo fieldInfo = type.GetField(variableName, BindingFlags.Public | BindingFlags.Instance);

        if (fieldInfo != null)
        {
            return fieldInfo.GetValue(obj);
        }

        PropertyInfo propertyInfo = type.GetProperty(variableName, BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo != null)
        {
            return propertyInfo.GetValue(obj);
        }

        throw new ArgumentException($"Variable '{variableName}' not found in class '{type.Name}'.");
    }

    public static T[] ConvertToList<T>(List<T> inputList)
    {
        if (inputList == null)
        {
            throw new ArgumentNullException(nameof(inputList));
        }

        return inputList.ToArray();
    }

    public static void SetVariableValue<T>(object obj, string variableName, T value)
    {

        Type classType = obj.GetType();
        FieldInfo field = classType.GetField(variableName, BindingFlags.Instance | BindingFlags.Public);
        Debug.Log(field.Name);
        if (field != null || value.ToString().Length > 0)
        {
            try
            {
                field.SetValue(obj, Convert.ChangeType(value, field.FieldType));
                // Debug.Log($"Variable '{variableName}' set to: {value}");
            }
            catch (FormatException)
            {
                Debug.Log("Wrong Format");

            }
        }
        else
        {
            Debug.Log($"Variable '{variableName}' not found or type mismatch.");
        }
    }
    public static void SetVariableValueDictionary<T>(object obj, Dictionary<string, object> data, string valuename, T value)
    {
        Type classType = obj.GetType();
        FieldInfo field = classType.GetField(valuename, BindingFlags.Instance | BindingFlags.Public);
        if (field != null || value.ToString().Length > 0)
        {
            try
            {
                data[valuename] = Convert.ChangeType(value, field.FieldType);
            }
            catch (FormatException)
            {
                Debug.Log("Wrong Format");

            }
        }
        else
        {
            Debug.Log($"Variable '{valuename}' not found or type mismatch.");
        }
    }

    public static void InterpolateVariableValueDictionary<T>(object obj, float startValue, ref Dictionary<string, object> data, string valuename, T targetValue, float startTime, float endTime, float valuereferenceTime, bool returnthis)
    {


        Debug.Log(obj);

        if (obj != null)
        {
            // Debug.Log(startValue); 
            // Debug.Log(valuereferenceTime + "/" + startTime + "/" + endTime+"/"+startValue+"/"+targetValue); 
            if (valuereferenceTime >= startTime && valuereferenceTime < endTime)
            {
                if (!returnthis)
                {
                    float t = Mathf.InverseLerp(startTime, endTime, valuereferenceTime);
                    float interpolatedValue = Mathf.Lerp(Convert.ToSingle(startValue), Convert.ToSingle(targetValue), t);
                    //Utility.SelfLerp(System.Convert.ToSingle(Utility.GetVariableValue(obj, "temp" + Convert.ToSingle(targetValue))), System.Convert.ToSingle(Utility.GetVariableValue(obj, targetValue.ToString())), 0.2f);
                    data[valuename] = interpolatedValue;
                    //field.SetValue(obj, interpolatedValue); 
                }
                else
                {
                    float t = Mathf.InverseLerp(startTime, endTime, valuereferenceTime);
                    float interpolatedValue = Mathf.Lerp(System.Convert.ToSingle(data[valuename]), Convert.ToSingle(targetValue), t);
                    data[valuename] = interpolatedValue;
                }

            }

            //field.SetValue(obj, targetValue);
            Debug.Log($"Variable '{startValue}' interpolated to: {targetValue}");
        }
        else
        {
            Debug.Log($"Variable '{startValue}' not found.");
        }
    }

    public static void InterpolateVariableValueField<T>(object obj, float startValue, FieldInfo valuename, T targetValue, float startTime, float endTime, float valuereferenceTime, bool returnthis)
    {


        Debug.Log(obj);

        if (obj != null)
        {
            // Debug.Log(startValue); 
            // Debug.Log(valuereferenceTime + "/" + startTime + "/" + endTime+"/"+startValue+"/"+targetValue); 
            if (valuereferenceTime >= startTime && valuereferenceTime < endTime)
            {
                if (!returnthis)
                {
                    float t = Mathf.InverseLerp(startTime, endTime, valuereferenceTime);
                    float interpolatedValue = Mathf.Lerp(Convert.ToSingle(startValue), Convert.ToSingle(targetValue), t);
                    //Utility.SelfLerp(System.Convert.ToSingle(Utility.GetVariableValue(obj, "temp" + Convert.ToSingle(targetValue))), System.Convert.ToSingle(Utility.GetVariableValue(obj, targetValue.ToString())), 0.2f);
                    Utility.SetVariableValue(obj, valuename.Name, interpolatedValue);
                    //field.SetValue(obj, interpolatedValue); 
                }
                else
                {
                    float t = Mathf.InverseLerp(startTime, endTime, valuereferenceTime);
                    float interpolatedValue = Mathf.Lerp(System.Convert.ToSingle(valuename.GetValue(obj)), Convert.ToSingle(targetValue), t);
                    Utility.SetVariableValue(obj, valuename.Name, interpolatedValue);

                }

            }

            //field.SetValue(obj, targetValue);
            Debug.Log($"Variable '{startValue}' interpolated to: {targetValue}");
        }
        else
        {
            Debug.Log($"Variable '{startValue}' not found.");
        }
    }
    public static float SelfLerp(float currentValue, float targetValue, float t)
    {
        return currentValue + (targetValue - currentValue) * t;
    }
    private static T Interpolate<T>(T startValue, T targetValue, float t)
    {
        if (typeof(T) == typeof(float))
        {
            float startFloat = Convert.ToSingle(startValue);
            float targetFloat = Convert.ToSingle(targetValue);
            return (T)(object)Mathf.Lerp(startFloat, targetFloat, t);
        }

        return default;
    }

    public static bool CompareArrayToList<T>(T[] array, List<T[]> list)
    {
        foreach (T[] item in list)
        {
            if (item.Length != array.Length)
                continue;

            bool match = true;
            for (int i = 0; i < item.Length; i++)
            {
                if (!item[i].Equals(array[i]))
                {
                    match = false;
                    break;
                }
            }

            if (match)
                return true;
        }

        return false;
    }
    public static bool CompareContainArrayToList<T>(T[] array, List<T[]> list)
    {
        foreach (T[] item in list)
        {
            if (item.Length != array.Length)
                continue;

            bool containsSameItems = true;
            for (int i = 0; i < item.Length; i++)
            {
                if (!array.Contains(item[i]))
                {
                    containsSameItems = false;
                    break;
                }
            }

            if (containsSameItems)
                return true;
        }

        return false;
    }
    public static bool CompareContainUnsortedArrayToList<T>(T[] array, List<T[]> list)
    {
        foreach (T[] item in list)
        {

            if (CompareUnsortedArrays(item, array))
            {
                return true;
            }



        }

        return false;
    }

    public static bool CompareArrays<T>(T[] array1, T[] array2)
    {
        if (array1.Length != array2.Length)
            return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (!array1[i].Equals(array2[i]))
                return false;
        }

        return true;
    }
    public static bool CompareUnsortedArrays<T>(T[] array1, T[] array2)
    {
        if (array1.Length != array2.Length)
            return false;

        // Convert arrays to lists for easy element comparison
        List<T> list1 = new List<T>(array1);
        List<T> list2 = new List<T>(array2);

        // Check if each element in list1 exists in list2
        int numofsimilar = list1.Count;
        foreach (T item in list1)
        {
            if (list2.Contains(item))
                numofsimilar -= 1;
        }


        if (numofsimilar == 0)
            return true;
        else
            return false;
    }
    #endregion

    #region Reflections
    public static Type[] GetClassesInObject(GameObject obj)
    {
        Component[] components = obj.GetComponents<Component>();
        Type[] classes = new Type[components.Length];
        for (int i = 0; i < components.Length; i++)
        {
            classes[i] = components[i].GetType();
        }
        return classes;
    }

    public static MethodInfo[] GetMethods(Type type)
    {
        return type.GetMethods(BindingFlags.Public | BindingFlags.Instance);


    }
    public static string[] ConvertArrayToList<T>(T[] array)
    {
        List<string> list = new List<string>();
        foreach (var item in array)
        {
            list.Add(item.ToString());
        }
        return list.ToArray();
    }

    public static MethodInfo GetMethodByName(Type classType, string methodName)
    {
        MethodInfo method = classType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

        return method;
    }
    public static Type GetTypeFromString(string typeName)
    {
        Type type = Type.GetType(typeName);

        return type;
    }
    public static bool HasParameters(MethodInfo method)
    {
        ParameterInfo[] parameters = method.GetParameters();
        return parameters.Length > 0;
    }
    public static ParameterInfo[] GetParameters(MethodInfo method)
    {
        ParameterInfo[] parameters = method.GetParameters();
        return parameters;
    }

    public static Type GetFirstParameterType(MethodInfo method)
    {
        ParameterInfo[] parameters = method.GetParameters();

        foreach (ParameterInfo parameter in parameters)
        {
            return parameter.ParameterType;
        }
        return null;
    }
    public static TypeCode GetTypeCode(Type type)
    {
        return Type.GetTypeCode(type);
    }

    public static PropertyInfo[] GetPropertyVariables(Type classType)
    {

        return classType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    }
    public static FieldInfo[] GetFieldVariables(Type classType)
    {
        return classType.GetFields(BindingFlags.Public | BindingFlags.Instance);

    }

    #endregion

    #region Reflection Set Get
    public static string[] GetClasses(object objectField, int selected)
    {

        return Utility.ConvertArrayToList(Utility.GetClassesInObject((GameObject)objectField));

    }
    public static string[] GetMethods(string classType, int selected)
    {


        Type type = Utility.GetTypeFromString(classType);
        return Utility.ConvertArrayToList(Utility.GetMethods(type));
    }


    public static void InvokeMethod(object targetObject, string className, string methodName, object[] parameters)
    {
        Type classType = Type.GetType(className);
        if (classType != null)
        {
            MethodInfo methodInfo = classType.GetMethod(methodName);
            if (methodInfo != null)
            {
                methodInfo.Invoke(targetObject, parameters);
            }
            else
            {
                Debug.Log("Method not found: " + methodName);
            }
        }
        else
        {
            Debug.Log("Class not found: " + className);
        }
    }
    #endregion

    #region File Edit

    public static void EditClassFile(string filePath, string fileName, string newContent)
    {

        //C:\Users\Marcg\Documents\TestV1.cs
        // Read the file content
        string fileContent = File.ReadAllText(filePath);

        // Create the regular expression pattern to match everything after the first "{"
        string pattern = @"{.*$";

        // Find the index of the first "{" in the file content
        int firstCurlyBracketIndex = fileContent.IndexOf('{');

        // If a "{" is found, replace the content after it with the new content
        if (firstCurlyBracketIndex != -1)
        {
            // Get the content before the first "{"
            string contentBeforeCurlyBracket = fileContent.Substring(0, firstCurlyBracketIndex);

            // Create the modified content with the new content enclosed in curly brackets
            string modifiedContent = contentBeforeCurlyBracket + "{\n" + newContent + "\n}";

            // Replace the file content with the modified content
            fileContent = modifiedContent;
        }

        // Write the modified content back to the file
        File.WriteAllText(filePath, fileContent);
    }

    public static void CreateAssetDirectory(string path)
    {
        string fullPath = Application.dataPath + path;
        Directory.CreateDirectory(fullPath);

        Debug.Log("Directory created at path: " + fullPath);
    }

    public static bool CheckAssetDirectoryExists(string path)
    {
        string fullPath = Application.dataPath + path;
        bool directoryExists = Directory.Exists(fullPath);

        if (directoryExists)
        {
            Debug.Log("Directory exists at path: " + fullPath);
        }
        else
        {
            Debug.Log("Directory does not exist at path: " + fullPath);
        }

        return directoryExists;
    }

    public static void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);

        Debug.Log("Directory created at path: " + path);
    }

    public static bool CheckDirectoryExists(string path)
    {
        bool directoryExists = Directory.Exists(path);

        if (directoryExists)
        {
            Debug.Log("Directory exists at path: " + path);
        }
        else
        {
            Debug.Log("Directory does not exist at path: " + path);
        }

        return directoryExists;
    }

    public static void WriteToFile(string filePath, string content)
    {
        // Check if the file path exists
        if (!File.Exists(filePath))
        {
            // Create the directory if it doesn't exist
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Create the file
            File.Create(filePath).Dispose();
        }

        // Write content to the file
        File.WriteAllText(filePath, content);
    }
    #endregion

    #region Arrays & Lists
    public static T[] CombineListOfArrays<T>(List<T[]> arrays)
    {
        // Calculate the total length of the combined array
        int totalLength = 0;
        foreach (T[] array in arrays)
        {
            totalLength += array.Length;
        }

        // Create the combined array
        T[] combinedArray = new T[totalLength];
        int currentIndex = 0;

        // Copy the elements from each array into the combined array
        foreach (T[] array in arrays)
        {
            Array.Copy(array, 0, combinedArray, currentIndex, array.Length);
            currentIndex += array.Length;
        }

        return combinedArray;
    }
    public static int FindArrayIndex<T>(T[] array, Predicate<T> match)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (match == null)
            throw new ArgumentNullException(nameof(match));

        for (int i = 0; i < array.Length; i++)
        {
            if (match(array[i]))
                return i;
        }

        return -1; // Return -1 if no matching element is found
    }

    public static int FindArrayIndex<T>(T[] array, T match)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (match == null)
            throw new ArgumentNullException(nameof(match));

        for (int i = 0; i < array.Length; i++)
        {
            if (match.Equals(array[i]))
                return i;
        }

        return -1; // Return -1 if no matching element is found
    }

    public static int FindArrayIndex<T>(List<T> array, T match)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (match == null)
            throw new ArgumentNullException(nameof(match));

        for (int i = 0; i < array.Count; i++)
        {
            if (match.Equals(array[i]))
                return i;
        }

        return -1; // Return -1 if no matching element is found
    }
    public static int FindListIndex<T>(IEnumerable<T> collection, Predicate<T> match)
    {
        //        int index = FindListIndex(names, x => x == "Alice");
        int index = 0;
        foreach (var item in collection)
        {
            if (match(item))
                return index;

            index++;
        }

        return -1; // Element not found
    }

    public static void ClearDestroyList<T>(List<T> list) where T : Component
    {
        foreach (T item in list)
        {
            UnityEngine.Object.Destroy(item);
        }
        list.Clear();
    }
    public static void ClearDestroyGameObjectList(List<GameObject> list)
    {
        foreach (var item in list)
        {
            UnityEngine.Object.Destroy(item);
        }
        list.Clear();
    }

    public static void ClearList<T>(List<T> list)
    {
        list.Clear();
    }

    public static void CheckList<T>(ref List<T> list)
    {
        if (list == null)
        {
            list = new List<T>();
        }
    }

    public static List<T> CheckList<T>(List<T> list)
    {
        if (list == null)
        {
            list = new List<T>();
        }
        return list;
    }

    public static string[] ConvertListToStringArray<T>(List<T> list)
    {
        string[] stringArray = new string[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            stringArray[i] = list[i].ToString();
        }
        return stringArray;
    }

    public static string[] ConvertEnumToStringArray<T>() where T : Enum
    {
        T[] enumValues = (T[])Enum.GetValues(typeof(T));
        string[] stringArray = new string[enumValues.Length];

        for (int i = 0; i < enumValues.Length; i++)
        {
            stringArray[i] = enumValues[i].ToString();
        }

        return stringArray;
    }

    public static T GetEnumValueFromInt<T>(int value) where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), value);
    }

    #endregion

    #region Mouse
    //for raycasts:
    //add:             RaycastHit hit;
    //before using function
    public static bool RayCastToMouse(Camera cam, out RaycastHit hit)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {

            return true;
        }
        return false;
    }

    public static bool RayCastForward(Transform obj, out RaycastHit hit)
    {
        Ray ray = new Ray(obj.position, obj.forward);

        if (Physics.Raycast(ray, out hit))
        {
            return true;
        }
        return false;
    }

    public static Vector3 RaycastPlane(Camera cam, Vector3 heightposition)
    {
        Vector3 hit = new Vector3();
        Vector3 mousePosition = Input.mousePosition;
        Plane horPlane = new Plane(Vector3.up, Vector3.Distance(cam.transform.position, heightposition));

        // Cast a ray from the camera to the mouse position
        Ray ray = cam.ScreenPointToRay(mousePosition);

        // Calculate the intersection point of the ray with the ground plane
        float rayDistance;
        if (horPlane.Raycast(ray, out rayDistance))
        {
            // Get the intersection point
            Vector3 intersectionPoint = ray.GetPoint(rayDistance);

            // Set the camera position to the intersection point with the desired height
            Vector3 targetPosition = new Vector3(intersectionPoint.x, Vector3.Distance(cam.transform.position, heightposition), intersectionPoint.z);
            hit = targetPosition;
        }
        return hit;
    }

    public static Vector3 RaycastCamera(Camera cam, float range)
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
            // Use hit.point as the position where the raycast hit something.
        }
        else
        {
            return ray.GetPoint(range);
            // Use defaultPosition as the position when the raycast doesn't hit anything.
        }
    }
    public static Vector3 RaycastDirection(Vector3 origin, Vector3 direction, float range, out RaycastHit hit)
    {
        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
            // Use hit.point as the position where the raycast hit something.
        }
        else
        {
            return ray.GetPoint(range);
            // Use defaultPosition as the position when the raycast doesn't hit anything.
        }
    }
    public static Vector3 GetPointInDirection(Vector3 origin, Vector3 direction, float range)
    {
        Ray ray = new Ray(origin, direction);
        return ray.GetPoint(range);

    }

    #endregion

    #region Mesh
    public static Mesh DuplicateMesh(Mesh originalMesh)
    {
        // Create a new instance of the original mesh
        Mesh duplicatedMesh = new Mesh();
        duplicatedMesh.name = originalMesh.name + "_copy";
        duplicatedMesh.vertices = originalMesh.vertices;
        duplicatedMesh.normals = originalMesh.normals;
        duplicatedMesh.uv = originalMesh.uv;
        duplicatedMesh.uv2 = originalMesh.uv2;
        duplicatedMesh.colors = originalMesh.colors;
        duplicatedMesh.tangents = originalMesh.tangents;
        duplicatedMesh.triangles = originalMesh.triangles;
        duplicatedMesh.subMeshCount = originalMesh.subMeshCount;

        // Copy submeshes (if any)
        for (int submeshIndex = 0; submeshIndex < originalMesh.subMeshCount; submeshIndex++)
        {
            int[] triangles = originalMesh.GetTriangles(submeshIndex);
            duplicatedMesh.SetTriangles(triangles, submeshIndex);
        }

        // Recalculate bounds and normals for the new mesh
        duplicatedMesh.RecalculateBounds();
        duplicatedMesh.RecalculateNormals();

        return duplicatedMesh;
    }
    public static bool IsPointInTriangle(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        Vector3 v0 = p1 - p0;
        Vector3 v1 = p2 - p0;
        Vector3 v2 = p - p0;

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        // Compute barycentric coordinates
        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        // Check if point is inside the triangle
        return (u >= 0) && (v >= 0) && (u + v < 1);
    }
    public static void SetVertices(GameObject obj, Vector3[] vertices)
    {
        // Check if the GameObject has a MeshFilter component
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter component not found on the GameObject.");
            return;
        }

        // Get the mesh of the GameObject
        Mesh mesh = meshFilter.mesh;

        // Check if the provided vertices array is not null and has the same length as the current mesh's vertices
        if (vertices == null || vertices.Length != mesh.vertexCount)
        {
            Debug.LogError("Invalid vertices array. It must have the same length as the current mesh's vertices.");
            return;
        }

        // Set the new vertices
        mesh.vertices = vertices;

        // Recalculate bounds and normals to update the mesh rendering
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
    public static void SetMesh(GameObject gameObject, Mesh mesh)
    {
        if (gameObject == null)
        {
            Debug.LogError("GameObject is null.");
            return;
        }

        if (mesh == null)
        {
            Debug.LogError("Mesh is null.");
            return;
        }

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        meshFilter.mesh = mesh;
    }
    public static Mesh GetMesh(GameObject gameObject)
    {
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            return meshFilter.mesh;
        }

        SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            return skinnedMeshRenderer.sharedMesh;
        }

        Debug.LogWarning("Mesh not found on GameObject: " + gameObject.name);
        return null;
    }
    public static MeshRenderer GetMeshRenderer(GameObject gameObject)
    {
        if (gameObject == null)
        {
            Debug.LogError("Invalid input: GameObject is null.");
            return null;
        }

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer component not found on the GameObject.");
        }

        return meshRenderer;
    }
    public static Vector3[] GetTriangleVertices(Mesh mesh, int triangleIndex)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // Calculate the starting index of the triangle in the triangles array
        int startIndex = triangleIndex * 3;

        // Get the vertex indices of the triangle
        int vertexIndex1 = triangles[startIndex];
        int vertexIndex2 = triangles[startIndex + 1];
        int vertexIndex3 = triangles[startIndex + 2];

        // Retrieve the vertices from the mesh using the indices
        Vector3 vertex1 = vertices[vertexIndex1];
        Vector3 vertex2 = vertices[vertexIndex2];
        Vector3 vertex3 = vertices[vertexIndex3];

        return new Vector3[] { vertex1, vertex2, vertex3 };
    }
    public static void FlipNormals(Mesh mesh)
    {
        // Reverse the winding order of each triangle
        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int temp = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = temp;
        }

        // Update the mesh triangles
        mesh.triangles = triangles;

        // Reverse the normals
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }

        // Update the mesh normals
        mesh.normals = normals;
    }
    public static Vector3[] CreateCube(float size)
    {
        Vector3[] points = new Vector3[8];
        points[0] = new Vector3(-size, -size, -size);
        points[1] = new Vector3(size, -size, -size);
        points[2] = new Vector3(size, -size, size);
        points[3] = new Vector3(-size, -size, size);
        points[4] = new Vector3(-size, size, -size);
        points[5] = new Vector3(size, size, -size);
        points[6] = new Vector3(size, size, size);
        points[7] = new Vector3(-size, size, size);
        return points;
    }
    public static void CreateMesh(Vector3[] points, GameObject obj)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = points;

        // Define triangles based on the vertices order
        int[] triangles = new int[(points.Length - 2) * 3];
        int index = 0;
        for (int i = 1; i < points.Length - 1; i++)
        {
            triangles[index++] = 0;
            triangles[index++] = i;
            triangles[index++] = i + 1;
        }
        mesh.triangles = triangles;

        // Create the MeshCollider component
        MeshCollider col = obj.GetComponent<MeshCollider>();
        if (col == null)
        {
            col = obj.AddComponent<MeshCollider>();
            col.sharedMesh = mesh;
        }
        else
        {
            col.sharedMesh = mesh;

        }
    }

    public static Vector3[] GetBoundingBoxCorners(Bounds bounds)
    {
        Vector3[] corners = new Vector3[8];

        // Get the corners of the bounding box
        corners[0] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        corners[1] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        corners[2] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[3] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
        corners[4] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        corners[5] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        corners[6] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[7] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);

        return corners;
    }
    public static Vector3[] GetBottomCorners(Bounds bounds)
    {
        Vector3[] corners = new Vector3[4];

        // Get the bottom corners of the bounding box
        corners[0] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        corners[1] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
        corners[2] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        corners[3] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);

        return corners;
    }
    public static Vector3[] GetTopCorners(Bounds bounds)
    {
        Vector3[] corners = new Vector3[4];

        // Get the top corners of the bounding box
        corners[0] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[1] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
        corners[2] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[3] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);

        return corners;
    }

    public static Vector3[] GetLeftSideCorners(Bounds bounds)
    {
        Vector3[] corners = new Vector3[4];

        // Get the left-side corners of the bounding box
        corners[0] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[1] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
        corners[2] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        corners[3] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);

        return corners;
    }

    public static Vector3[] GetRightSideCorners(Bounds bounds)
    {
        Vector3[] corners = new Vector3[4];

        // Get the right-side corners of the bounding box
        corners[0] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
        corners[1] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
        corners[2] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
        corners[3] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);

        return corners;
    }

    #endregion

    #region LineRenderer
    public static void SetLineRenderer(LineRenderer line, List<Vector3> positions)
    {
        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());
    }
    #endregion

    #region Boost
    public static bool CheckEffects(Dictionary<BoostScriptables, float> effects, BoostScriptables boost)
    {
        if (effects.ContainsKey(boost))
        {
            return true;
        }
        else
            return false;
    }
    #endregion

    #region RayCast
    public static bool RaycastToPositions(Vector3 origin, Vector3 target, out RaycastHit hitInfo)
    {
        Vector3 direction = target - origin;
        float distance = direction.magnitude;

        Ray ray = new Ray(origin, direction);
        return Physics.Raycast(ray, out hitInfo, distance);
    }
    public static RaycastHit GetRaycastHitFromObject(Camera cam, GameObject targetObject)
    {
        RaycastHit hit;

        // Calculate the raycast direction from the camera to the target object
        Vector3 raycastDirection = targetObject.transform.position - cam.transform.position;

        // Create a ray from the camera position and the calculated direction
        Ray ray = new Ray(cam.transform.position, raycastDirection);

        // Perform the raycast and check if it hits the target object
        if (Physics.Raycast(ray, out hit, 100))
        {
            // Object hit, return the RaycastHit
            return hit;
        }
        else
        {
            // Object not hit, return an empty RaycastHit
            return new RaycastHit();
        }
    }
    public static RaycastHit GetRaycastHitFromTopOfObject(Camera cam, GameObject targetObject, float offset)
    {
        RaycastHit hit;

        // Calculate the raycast direction from the camera to the target object
        Bounds bound = targetObject.GetComponent<Renderer>().bounds;
        Vector3 pos = new Vector3(0, bound.size.y + offset, 0) + targetObject.transform.position;

        // Create a ray from the camera position and the calculated direction
        Ray ray = new Ray(pos, Vector3.down * bound.size.y * 2);

        // Perform the raycast and check if it hits the target object
        if (Physics.Raycast(ray, out hit, 100))
        {
            // Object hit, return the RaycastHit
            return hit;
        }
        else
        {
            // Object not hit, return an empty RaycastHit
            return new RaycastHit();
        }
    }
    #endregion

    #region Line Intersect
    public static bool LineIntersection(out Vector3 intersection,
        Vector3 linePoint1, Vector3 lineDirection1,
        Vector3 linePoint2, Vector3 lineDirection2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineDirection1, lineDirection2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineDirection2);
        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        //if (Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
        //if (Mathf.Abs(planarFactor) < 0.1f && crossVec1and2.sqrMagnitude > 0.1f)
        if (Mathf.Abs(planarFactor) < 0.1f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineDirection1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }
    public static void LineIntersection(Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Vector3 p0, out Vector3 p1, float marginOfError, out bool onSegment, out bool intersects)
    {
        Vector3 r = b - a;
        Vector3 s = d - c;
        Vector3 q = a - c;

        float dotqr = Vector3.Dot(q, r);
        float dotqs = Vector3.Dot(q, s);
        float dotrs = Vector3.Dot(r, s);
        float dotrr = Vector3.Dot(r, r);
        float dotss = Vector3.Dot(s, s);

        float denom = dotrr * dotss - dotrs * dotrs;
        float numer = dotqs * dotrs - dotqr * dotss;

        float t = numer / denom;
        float u = (dotqs + t * dotrs) / dotss;

        // The two points of intersection
        p0 = a + t * r;
        p1 = c + u * s;

        // Is the intersection occuring along both line segments and does it intersect
        onSegment = false;
        intersects = false;
        if (0 <= t && t <= 1 && 0 <= u && u <= 1) onSegment = true;
        if ((p0 - p1).magnitude <= marginOfError) intersects = true;
    }

    public static bool ThreeLineIntersection(out Vector3 intersection,
        Vector3 linePoint1, Vector3 lineDirection1,
        Vector3 linePoint2, Vector3 lineDirection2,
        Vector3 linePoint3, Vector3 lineDirection3)
    {
        Vector3 crossVec1and2 = Vector3.Cross(lineDirection1, lineDirection2);
        Vector3 crossVec1and3 = Vector3.Cross(lineDirection1, lineDirection3);
        Vector3 crossVec2and3 = Vector3.Cross(lineDirection2, lineDirection3);

        float denominator = Vector3.Dot(lineDirection1, crossVec2and3);

        // Check if the lines are not parallel
        if (Mathf.Abs(denominator) > 0.0001f)
        {
            Vector3 lineVec3 = linePoint3 - linePoint1;
            float s1 = Vector3.Dot(crossVec2and3, lineVec3) / denominator;

            intersection = linePoint1 + (lineDirection1 * s1);

            // Check if the intersection point is also on the other two lines
            float s2 = Vector3.Dot(crossVec1and3, lineVec3) / denominator;
            float s3 = Vector3.Dot(crossVec1and2, lineVec3) / denominator;

            if (Mathf.Abs(Vector3.Dot(lineDirection2, crossVec1and3) * s2 - Vector3.Dot(lineDirection3, crossVec1and2) * s3) < 0.0001f)
            {
                return true;
            }
        }

        intersection = Vector3.zero;
        return false;
    }

    #endregion

    #region Rendering

    public static Texture2D ApplyEdgeDetectionFilter(Texture2D inputImage, float threshold)
    {
        // Create a new texture for the filtered image
        Texture2D filteredImage = new Texture2D(inputImage.width, inputImage.height);

        // Create a grayscale version of the input image
        Texture2D grayscaleImage = ConvertToGrayscale(inputImage);

        // Loop through each pixel in the grayscale image
        for (int y = 1; y < grayscaleImage.height - 1; y++)
        {
            for (int x = 1; x < grayscaleImage.width - 1; x++)
            {
                // Apply the Sobel operator for edge detection
                float gx = CalculateGradientX(grayscaleImage, x, y);
                float gy = CalculateGradientY(grayscaleImage, x, y);

                // Calculate the gradient magnitude
                float gradientMagnitude = Mathf.Sqrt(gx * gx + gy * gy);

                // Apply thresholding to determine edges
                UnityEngine.Color edgeColor = gradientMagnitude >= threshold ? UnityEngine.Color.white : UnityEngine.Color.black;

                // Set the corresponding pixel in the filtered image
                filteredImage.SetPixel(x, y, edgeColor);
            }
        }

        // Apply the changes to the filtered image
        filteredImage.Apply();

        return filteredImage;
    }

    public static Texture2D ConvertToGrayscale(Texture2D inputTexture)
    {
        // Create a new texture for the grayscale image
        Texture2D grayscaleTexture = new Texture2D(inputTexture.width, inputTexture.height);

        // Loop through each pixel in the input texture
        for (int y = 0; y < inputTexture.height; y++)
        {
            for (int x = 0; x < inputTexture.width; x++)
            {
                // Get the color of the pixel in the input texture
                UnityEngine.Color pixelColor = inputTexture.GetPixel(x, y);

                // Calculate the grayscale value
                float grayscaleValue = (pixelColor.r + pixelColor.g + pixelColor.b) / 3f;

                // Create a new grayscale color
                UnityEngine.Color grayscaleColor = new UnityEngine.Color(grayscaleValue, grayscaleValue, grayscaleValue);

                // Set the corresponding pixel in the grayscale image
                grayscaleTexture.SetPixel(x, y, grayscaleColor);
            }
        }

        // Apply the changes to the grayscale image
        grayscaleTexture.Apply();

        return grayscaleTexture;
    }

    private static float CalculateGradientX(Texture2D texture, int x, int y)
    {
        return texture.GetPixel(x - 1, y - 1).grayscale * -1 +
               texture.GetPixel(x - 1, y).grayscale * -2 +
               texture.GetPixel(x - 1, y + 1).grayscale * -1 +
               texture.GetPixel(x + 1, y - 1).grayscale +
               texture.GetPixel(x + 1, y).grayscale * 2 +
               texture.GetPixel(x + 1, y + 1).grayscale;
    }

    private static float CalculateGradientY(Texture2D texture, int x, int y)
    {
        return texture.GetPixel(x - 1, y - 1).grayscale * -1 +
               texture.GetPixel(x, y - 1).grayscale * -2 +
               texture.GetPixel(x + 1, y - 1).grayscale * -1 +
               texture.GetPixel(x - 1, y + 1).grayscale +
               texture.GetPixel(x, y + 1).grayscale * 2 +
               texture.GetPixel(x + 1, y + 1).grayscale;
    }

    public static Texture2D ApplyVintageFilter(Texture2D inputImage)
    {
        // Create a new texture for the filtered image
        Texture2D filteredImage = new Texture2D(inputImage.width, inputImage.height);

        // Loop through each pixel in the input image
        for (int y = 0; y < inputImage.height; y++)
        {
            for (int x = 0; x < inputImage.width; x++)
            {
                // Get the color of the pixel in the input image
                UnityEngine.Color pixelColor = inputImage.GetPixel(x, y);

                // Apply the vintage effect
                UnityEngine.Color vintageColor = new UnityEngine.Color(
                Mathf.Clamp01(pixelColor.r * 0.8f), // Red channel
                Mathf.Clamp01(pixelColor.g * 0.9f), // Green channel
                Mathf.Clamp01(pixelColor.b * 0.7f), // Blue channel
                pixelColor.a // Alpha channel
            );

                // Set the corresponding pixel in the filtered image
                filteredImage.SetPixel(x, y, vintageColor);
            }
        }

        // Apply the changes to the filtered image
        filteredImage.Apply();

        return filteredImage;
    }

    public static Texture2D ApplyPatternReplacementFilter(Texture2D inputImage, Texture2D patternImage)
    {
        // Create a new texture for the filtered image
        Texture2D filteredImage = new Texture2D(inputImage.width, inputImage.height);

        // Loop through each pixel in the input image
        for (int y = 0; y < inputImage.height; y++)
        {
            for (int x = 0; x < inputImage.width; x++)
            {
                // Get the color of the pixel in the input image
                UnityEngine.Color pixelColor = inputImage.GetPixel(x, y);

                // Check if the pixel is black
                if (pixelColor == UnityEngine.Color.black)
                {
                    // Calculate the coordinates in the pattern image based on the pixel position
                    int patternX = x % patternImage.width;
                    int patternY = y % patternImage.height;

                    // Get the color of the corresponding pixel in the pattern image
                    UnityEngine.Color patternColor = patternImage.GetPixel(patternX, patternY);

                    // Set the corresponding pixel in the filtered image to the pattern color
                    filteredImage.SetPixel(x, y, patternColor);
                }
                else
                {
                    // Set the corresponding pixel in the filtered image to the original color
                    filteredImage.SetPixel(x, y, pixelColor);
                }
            }
        }

        // Apply the changes to the filtered image
        filteredImage.Apply();

        return filteredImage;
    }

    public static Texture2D CaptureFlatColorScreenshot(Camera captureCamera)
    {
        // Create a RenderTexture with the same dimensions as the camera's viewport
        RenderTexture renderTexture = new RenderTexture(captureCamera.pixelWidth, captureCamera.pixelHeight, 24);

        // Set the camera's target texture to the RenderTexture
        captureCamera.targetTexture = renderTexture;

        // Create a new Texture2D to store the screenshot
        Texture2D screenshot = new Texture2D(captureCamera.pixelWidth, captureCamera.pixelHeight);

        // Render the camera's view into the RenderTexture
        captureCamera.Render();

        // Set the active RenderTexture to read the pixels from
        RenderTexture.active = renderTexture;

        // Read the pixels from the RenderTexture into the screenshot texture
        screenshot.ReadPixels(new Rect(0, 0, captureCamera.pixelWidth, captureCamera.pixelHeight), 0, 0);
        screenshot.Apply();

        // Reset the camera's target texture and active RenderTexture
        captureCamera.targetTexture = null;
        RenderTexture.active = null;

        // Destroy the temporary RenderTexture
        GameObject.Destroy(renderTexture);

        return screenshot;
    }



    #endregion

    #region Materials

    public static Material UnlitColor(UnityEngine.Color color)
    {
        Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        newMaterial.color = color;
        return newMaterial;
    }

    #endregion

    #region Resources

    public static T LoadResourceByName<T>(string resourceName) where T : UnityEngine.Object
    {
        T resource = Resources.Load<T>(resourceName);
        return resource;
    }

    public static T LoadResourceByPath<T>(string resourcePath) where T : UnityEngine.Object
    {
        T resource = Resources.Load<T>(resourcePath);
        return resource;
    }

    public static T LoadDefaultResource<T>(string resourceName) where T : UnityEngine.Object
    {
        T resource = Resources.Load<T>("unity_builtin_extra/" + resourceName);
        Debug.Log(resource);
        return resource;
    }
    #endregion

    #region Animator

    public static void AnimateMoveDirectionalIndpendent(Transform from, Vector3 movedirection, Animator anim)
    {
        float angle = Vector3.Angle(from.forward, movedirection);
        bool right = false;
        Vector3 cross = Vector3.Cross(from.forward, movedirection);
        if (cross.y < 0) right = false;
        if (cross.y > 0) right = true;
        if (movedirection.magnitude >= 0.1f)
        {


            if (angle <= 22.5f)
            {
                anim.SetBool("forward", true);
                anim.SetBool("back", false);
                anim.SetBool("left", false);
                anim.SetBool("right", false);
            }
            else if (angle <= 67.5 && angle > 22.5f)
            {
                if (right)
                {
                    anim.SetBool("forward", true);
                    anim.SetBool("back", false);
                    anim.SetBool("left", false);
                    anim.SetBool("right", true);
                }
                else if (right == false)
                {
                    anim.SetBool("forward", true);
                    anim.SetBool("back", false);
                    anim.SetBool("left", true);
                    anim.SetBool("right", false);
                }
            }
            else if (angle <= 112.5 && angle > 67.5f)
            {
                if (right)
                {
                    anim.SetBool("forward", false);
                    anim.SetBool("back", false);
                    anim.SetBool("left", false);
                    anim.SetBool("right", true);
                }
                else if (right == false)
                {
                    anim.SetBool("forward", false);
                    anim.SetBool("back", false);
                    anim.SetBool("left", true);
                    anim.SetBool("right", false);
                }
            }
            else if (angle <= 157.5 && angle > 112.5f)
            {
                if (right)
                {
                    anim.SetBool("forward", false);
                    anim.SetBool("back", true);
                    anim.SetBool("left", false);
                    anim.SetBool("right", true);
                }
                else if (right == false)
                {
                    anim.SetBool("forward", false);
                    anim.SetBool("back", true);
                    anim.SetBool("left", true);
                    anim.SetBool("right", false);
                }
            }
            else if (angle > 157.5)
            {
                anim.SetBool("forward", false);
                anim.SetBool("back", true);
                anim.SetBool("left", false);
                anim.SetBool("right", false);


            }



        }
        else
        {
            anim.SetBool("forward", false);
            anim.SetBool("back", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
    }

    #endregion

    #region Grid
    public static List<Vector3> GenerateGrid(Vector3 startPosition, Vector3 gridCellSize, Vector3Int gridCellCount)
    {

        List<Vector3> gridPositions = new List<Vector3>();

        Vector3 gridCenter = startPosition;

        float startX = gridCenter.x - (gridCellCount.x - 1) * gridCellSize.x * 0.5f;
        float startY = gridCenter.y - (gridCellCount.y - 1) * gridCellSize.y * 0.5f;
        float startZ = gridCenter.z - (gridCellCount.z - 1) * gridCellSize.z * 0.5f;

        for (int z = 0; z < gridCellCount.z; z++)
        {
            for (int y = 0; y < gridCellCount.y; y++)
            {
                for (int x = 0; x < gridCellCount.x; x++)
                {
                    float posX = startX + x * gridCellSize.x;
                    float posY = startY + y * gridCellSize.y;
                    float posZ = startZ + z * gridCellSize.z;

                    Vector3 gridPosition = new Vector3(posX, posY, posZ);
                    gridPositions.Add(gridPosition);
                }
            }
        }

        return gridPositions;
    }

    public static List<Vector3> GenerateGridBrick(Vector3 startPosition, Vector3 gridCellSize, Vector3Int gridCellCount, float yOffset)
    {
        List<Vector3> gridPositions = new List<Vector3>();

        Vector3 gridCenter = startPosition;

        float startX = gridCenter.x - (gridCellCount.x - 1) * gridCellSize.x * 0.5f;
        float startY = gridCenter.y - (gridCellCount.y - 1) * gridCellSize.y * 0.5f;
        float startZ = gridCenter.z - (gridCellCount.z - 1) * gridCellSize.z * 0.5f;

        bool isOffset = false; // Flag to track the alternating offset

        for (int z = 0; z < gridCellCount.z; z++)
        {
            for (int y = 0; y < gridCellCount.y; y++)
            {
                isOffset = !isOffset; // Toggle the offset flag for each Y level

                for (int x = 0; x < gridCellCount.x; x++)
                {
                    float posX = startX + x * gridCellSize.x;
                    float posY = startY + y * gridCellSize.y;
                    float posZ = startZ + z * gridCellSize.z;

                    if (isOffset) // Apply the offset on alternating Y levels (except for the first level)
                        posX += 0.5f * gridCellSize.x;

                    Vector3 gridPosition = new Vector3(posX, posY + yOffset * y, posZ);
                    gridPositions.Add(gridPosition);
                }
            }
        }

        return gridPositions;
    }

    public static List<Vector3> GenerateGridClosed(Vector3 startPosition, Vector3 gridSize, Vector3Int count, out Vector3 cellSize)
    {
        List<Vector3> gridPositions = new List<Vector3>();
        cellSize = new Vector3(gridSize.x / count.x, gridSize.y / count.y, gridSize.z / count.z);
        Vector3 gridCenter = startPosition + new Vector3((count.x - 1) * -cellSize.x * 0.5f, (count.y - 1) * -cellSize.y * 0.5f, (count.z - 1) * -cellSize.z * 0.5f);


        for (int x = 0; x < count.x; x++)
        {
            for (int y = 0; y < count.y; y++)
            {
                for (int z = 0; z < count.z; z++)
                {
                    gridPositions.Add(gridCenter + new Vector3(x * cellSize.x, y * cellSize.y, z * cellSize.z));
                }
            }
        }

        return gridPositions;
    }

    public static List<Vector3> GenerateGridBrickPattern(Vector3 startPosition, Vector3 gridSize, Vector3Int count, out Vector3 cellSize)
    {
        List<Vector3> gridPositions = new List<Vector3>();
        cellSize = new Vector3(gridSize.x / count.x, gridSize.y / count.y, gridSize.z / count.z);
        Vector3 gridCenter = startPosition + new Vector3((count.x - 1) * -cellSize.x * 0.5f, (count.y - 1) * -cellSize.y * 0.5f, (count.z - 1) * -cellSize.z * 0.5f);

        bool offset = false;

        for (int y = 0; y < count.y; y++)
        {
            for (int x = 0; x < count.x; x++)
            {
                for (int z = 0; z < count.z; z++)
                {
                    Vector3 position = gridCenter + new Vector3(x * cellSize.x, y * cellSize.y, z * cellSize.z);
                    if (offset)
                    {
                        position += new Vector3(cellSize.x * 0.5f, 0f, 0f);
                    }
                    gridPositions.Add(position);
                }
            }
            offset = !offset; // Toggle the offset for each Y level
        }

        return gridPositions;
    }

    public static Vector3 GetNearestCell(Vector3 position, Vector3 gridCenter, Vector3 cellSize)
    {
        Vector3 relativePosition = position - gridCenter;
        Vector3 snappedPosition = new Vector3(
            Mathf.Round(relativePosition.x / cellSize.x) * cellSize.x,
            Mathf.Round(relativePosition.y / cellSize.y) * cellSize.y,
            Mathf.Round(relativePosition.z / cellSize.z) * cellSize.z
        );
        return snappedPosition + gridCenter;
    }


    #endregion

    #region Gizmos

    public static void DisplayMesh(Mesh mesh, Transform transform)
    {
        if (mesh == null)
            return;

        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawMesh(mesh);
    }

    public static void DrawListVector(List<Vector3> pos, bool loopToStart)
    {
        for (int i = 0; i < pos.Count; i++)
        {
            Vector3 startPos = pos[i];
            Vector3 endPos = pos[loopToStart ? (i + 1) % pos.Count : Mathf.Min(i + 1, pos.Count - 1)];

            Gizmos.DrawLine(startPos, endPos);
        }
    }
    #endregion

    #region Debug

    public static void DrawDebugLines(List<Vector3> positions)
    {
        if (positions.Count < 2)
        {
            return;
        }

        for (int i = 0; i < positions.Count - 1; i++)
        {
            Debug.DrawLine(positions[i], positions[i + 1], UnityEngine.Color.red);
        }
    }
    #endregion

    #region Navmesh

    public static void MoveToNearestNavMeshPoint(NavMeshAgent agent)
    {
        NavMeshHit hit;
        if (!agent.isOnNavMesh)
        {
            // Agent is outside the NavMesh or no valid position found
            // Find the nearest point on the NavMesh and move the agent there
            if (NavMesh.FindClosestEdge(agent.transform.position, out hit, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
            else
            {
                Debug.LogWarning("No valid position on NavMesh found!");
            }
        }
    }

    public static void MoveToNearestNavMeshEdge(NavMeshAgent agent)
    {
        NavMeshHit hit;
        if (!agent.isOnNavMesh)
        {
            if (NavMesh.FindClosestEdge(agent.transform.position, out hit, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
            else
            {
                Debug.LogWarning("No valid position on NavMesh found!");
            }
        }
    }

    public static Vector3 GetNearestNavMeshPoint(Vector3 position)
    {

        NavMeshHit hit;

        if (NavMesh.SamplePosition(position, out hit, 100, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            return Vector3.zero;
        }


    }

    public static Vector3 GetNearestNavMeshEdge(Vector3 position)
    {
        NavMeshHit hit;

        if (NavMesh.FindClosestEdge(position, out hit, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
            return Vector3.zero;



    }

    public static bool CheckIfInNavMesh(Vector3 position)
    {
        NavMeshHit navMeshHit;
        bool isPositionInNavMesh = NavMesh.SamplePosition(position, out navMeshHit, 1, NavMesh.AllAreas);
        Debug.Log(isPositionInNavMesh);
        return isPositionInNavMesh;
    }

    #endregion

    #region Codes
    public static string FPSController()
    {
        return @"
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FPS_Controller : MonoBehaviour
{
    public float sensitivity = 2f; 
    private float xRotation = 0f;
    private float desiredX = 0f;
    #region Wallrun Values
    [Header(""Wallrun"")]
    public LayerMask wall;
    public float wallrunForce=200 ;
    public float wallcheckdistance=0.7f ;
    public float minjumpheight =2;
    public float walljumpupforce=7;
    public float walljumpsideforce=12;
    bool iswallright, iswallleft;
    RaycastHit leftwallhit, rightwallhit;
    bool iswallrunning;
    private Rigidbody rb;
    public Camera cam;
    public float maxwallruncameratilt=25;
        private float wallruncameratilt;
    public float wallrunspeed=8.5f;
    private bool upwardsrunning;
    private bool downwardrunning;
    public float wallclimbspeed=3;
    private bool exitingwallrun;
    public float exitwallruntime=0.2f;
    private float exitwallruntimer;
    private float wallruntimer;
    public float maxwallrunTime=0.7f;

    public bool usewallrungravity;
    public float gravitycounterforce;


    #endregion

    #region Movement Values
    [Header(""Movement"")]
    private float movespeed;
    public float walkspeed=7;
    public float runspeed=15;
    public Transform orientation;
    public float desiredMoveSpeed;
    public float lastDesiredMovespeed;
    public float maxYspeed;


    float horizontalInput;
     float verticalInput;
    Vector3 moveDirection;
    public MoveState state;

    public bool freeze;
    public bool unlimited;
    public bool restricted;
    #endregion

    #region Ground Check Values
    [Header(""Ground Check"")]
    public Transform checker;
    public LayerMask ground;
    bool isgrounded;
    public float grounddrag=5;
    #endregion

    #region Air Jump Values
    [Header(""Jump/Air"")]
    public float jumpForce=12;
    public float jumpCooldown=0.5f;
    public float airmultiplier=0.4f;

    private int doubleJumpsLeft;
    public int startDoubleJumps=2;

    bool readyToJump = true;

    #endregion

    #region Crouch VAlues
    [Header(""Crouching"")]
    public float crouchspeed=3.5f;
    public float crouchYscale=0.5f;
    private float startYScale;
    #endregion

    #region Slope Values
    [Header(""Slope Handling"")]
    public float maxSlopeAngle=40;
    private RaycastHit slopeHit;
    private bool exitingslope;
    #endregion

    #region Climbing Values
    [Header(""Climbing"")]
    public float climbSpeed=10;
    public float maxClimbTime=0.75f;
    private float climbTimer;
    private bool climbing;
    
    [Header(""Climbing detection"")]
    public float detectionLength=0.7f;
    public float sphereCastRadius=0.5f;
    public float maxWallLookAngle=30;
    private float wallLookAngle;
    private RaycastHit frontWallHit;
    private bool wallfront;

    [Header(""Climb Jump"")]
    public float climbJumpUPForce=14;
    public float climbJumpBackForce=12;
    public int climbjumps=1;
    private int climbjumpsleft;
    private Transform lastwall;
    private Vector3 lastwallnormal;
    public float minwallnormalanglechange=5;

    public bool exitingwall;
    public float exitwalltime;
    private float exitwalltimer;
    #endregion

    #region Ledge Values
    [Header(""LedgeGrab"")]
    public float ledgedetectionLength=3;
    public float ledgeSphereCastRadius=0.5f;
    public LayerMask ledge;

    public Transform lastLedge;
    public Transform currledge;
    private RaycastHit ledgehit;

    public float moveToLedgeSpeed=12;
    public float maxLedgeGrabDistance=2;
    public float minTimeOnLedge=5;
    private float timeOnLedge;
    public bool holding;

    public float ledgejumpforwardforce=14;
    public float ledgejumpupforce=5;
    public bool exitingledge;
    public float exitledgetime=0.2f;
    private float exitledgetimer;
    #endregion

    #region Slide Values
    [Header(""Sliding"")]
    public float maxSlideTime=0.75f;
    public float slideForce=200;
    private float slideTimer;

    public float slideYScale=0.5f;
    public float slidespeed=30;
    private float slidestartYScale;

    private bool sliding;
    #endregion

    #region Dashing Values
    [Header(""Dashing"")]
    public float dashforce=100;
    public float dashspeed=20;
    public float dashupwardforce;
    public float dashduration=0.25f;
    public float dashcd=1.5f;
    private float dashcdtimer;
    public bool dashing;
    private Vector3 dashdelayedforcetoapply;
    public bool useCameraForward = true;
    public bool allowalldirections = true;
    public bool disablegravity;
    public bool resetvel = true;
    public float maxDashYspeed=15;
    public float dashfov=95;
    #endregion

    #region Grappling Values
    [Header(""Grappling"")]
    public List<Transform> guntip;
    public LayerMask grappleable;
    public float maxgrapplingdistance=25;
    public float grappledelaytime=0.25f;

   // private List<Vector3> grapplepoint;
    public float grapplingcd=1;
    private float grapplingcdtimer;
    private bool grappling;
    public List<LineRenderer> lr;
    private bool activegrapple;
    private bool enablemovementonnexttouch;
    private Vector3 velocitytoset;
    public float overshootYaxis=2;
    public float grapplefov=85;
    #endregion

    #region Swinging Values

    [Header(""Swinging"")]
    public float maxswingdistance = 25f;
    private List<Vector3> swingpoint;
    private List<SpringJoint> joint;
    private List<Vector3> currentgrappleposition;
    public float swingspeed=15;
    private bool swinging;

    public float horizontalthrustforce=2000;
    public float forwardthrustforce=3000;
    public float extendcablespeed=20;

    private List<RaycastHit> predictionhit;
    public float predictionSphereCastRadius=3;
    public List<Transform> predictionPoint;
    #endregion

    #region DualGrapple
    [Header(""Dual"")]
    public int amountofswingpoints = 2;
    public List<Transform> pointaimers;
    public List<bool> swingsactive;
    public List<bool> grappleactive;
    private Vector3 pullpoint;
    #endregion
    // private bool keepMomentum;


    #region Callers
    private void Start()
    {
        orientation = transform;
        rb = Utility.GetOrAddComponent<Rigidbody>(this.gameObject);
        rb.freezeRotation = true;
        readyToJump = true;
        startYScale = transform.localScale.y;
        slidestartYScale = transform.localScale.y;
        Cursor.lockState = CursorLockMode.Locked;
        ListSetup();
    }
    public void Update()
    {
        isgrounded = Physics.CheckSphere(checker.position, 0.2f, ground);
        MyInput();
        SpeedControl();
        StateHandler();
        if ((state==MoveState.Walk||state==MoveState.Run||state==MoveState.Crouch)&&!activegrapple)
            rb.drag = grounddrag;
        else
            rb.drag = 0;

        WallrunState();
        CheckForWall();

        WallCheck();
        ClimbingState();
        if (climbing&&!exitingwall)
            ClimbingMovement();

        LedgeDetection();
        LedgeStateMachine();

        if(Input.GetKeyDown(KeyCode.F))
        {
            Dash();
        }
        if (dashcdtimer > 0)
            dashcdtimer -= Time.deltaTime;

        MyInput2();
        CheckForSwingPoints();

        if (grapplingcdtimer > 0)
            grapplingcdtimer -= Time.deltaTime;

        
        if (joint[0] != null||joint[1] != null) OdmGearMovement();

    }
    private void FixedUpdate()
    {
        MovePlayer();
        if (iswallrunning)
            Wallrunning();
        if (sliding)
            Sliding();
    }
    private void LateUpdate()
    {
        
        DrawRope();
    }

    private void MyInput2()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetMouseButtonDown(1)) StartSwing(0);
            if (Input.GetMouseButtonDown(0)) StartSwing(1);

        }
        else
        {
            if (Input.GetMouseButtonDown(1)) StartGrapple(0);
            if (Input.GetMouseButtonDown(0)) StartGrapple(1);

        }
        if (Input.GetMouseButtonUp(1)) StopSwing(0);
            if (Input.GetMouseButtonUp(0)) StopSwing(1);

    }
    #endregion

    #region Advance Movement

    private void StateHandler()
    {
        if(freeze)
        {
            state = MoveState.freeze;
            rb.velocity = Vector3.zero;
            movespeed = 0;
        }
        else if(unlimited)
        {
            state = MoveState.unlimited;
            desiredMoveSpeed = 999f;
        }
        else if (swinging)
        {
            state = MoveState.swinging;
            desiredMoveSpeed = swingspeed;
        }
       else if(dashing)
        {
            state = MoveState.dashing;
            desiredMoveSpeed = dashspeed;
        }
        else if(iswallrunning)
        {
            state = MoveState.Wallrunning;
            desiredMoveSpeed = wallrunspeed;
        }
        else if(sliding)
        {
            state = MoveState.Sliding;
            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slidespeed;
            else
                desiredMoveSpeed = runspeed;

        }
        else if(climbing)
        {
            state = MoveState.Climbing;
            desiredMoveSpeed = climbSpeed;
        }
       else if (Input.GetKey(KeyCode.LeftControl))
        {
            state = MoveState.Crouch;
            desiredMoveSpeed = crouchspeed;
        }
        else if (isgrounded&&Input.GetKey(KeyCode.LeftShift))
        {
            state = MoveState.Run;
            desiredMoveSpeed = runspeed;
        }
        else if(isgrounded)
        {
            state = MoveState.Walk;
            desiredMoveSpeed = walkspeed;
        }
       
        else
        {
            state = MoveState.Air;
        }
        if(Mathf.Abs(desiredMoveSpeed-lastDesiredMovespeed)>4f&&movespeed!=0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothLerpMoveSpeed());
        }
        else
        {
            movespeed = desiredMoveSpeed;
        }
        lastDesiredMovespeed = desiredMoveSpeed;
    }
    private IEnumerator SmoothLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - movespeed);
        float startValue = movespeed;
        while(time<difference)
        {
            movespeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            yield return null;
        }
        movespeed = desiredMoveSpeed;
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(checker.position,Vector3.down,out slopeHit,0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private Vector3 GetSlopeMoveDirection(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    #endregion

    #region Movement
    private void MyInput()
    {
        float mouseX = Input.GetAxis(""Mouse X"") * sensitivity;
        float mouseY = Input.GetAxis(""Mouse Y"") * sensitivity;


        Vector3 rot = cam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, mouseX, wallruncameratilt);
        orientation.transform.Rotate(Vector3.up*desiredX);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        horizontalInput = Input.GetAxisRaw(""Horizontal"");
        verticalInput = Input.GetAxisRaw(""Vertical"");

        if (Input.GetKeyDown(KeyCode.LeftAlt)&&(horizontalInput!=0||verticalInput!=0))
        {
            StartSliding();
        }
        if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            StopSliding();
        }
        if(Input.GetKeyDown(KeyCode.Space)&&readyToJump&&isgrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYscale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        if (MathF.Abs(wallruncameratilt) < maxwallruncameratilt && iswallrunning && iswallright)
            wallruncameratilt += Time.deltaTime * maxwallruncameratilt * 2;
         if (MathF.Abs(wallruncameratilt) < maxwallruncameratilt && iswallrunning && iswallleft)
            wallruncameratilt -= Time.deltaTime * maxwallruncameratilt * 2;


        if (wallruncameratilt > 0 && !iswallright && !iswallleft)
            wallruncameratilt -= Time.deltaTime * maxwallruncameratilt * 2;
        if (wallruncameratilt < 0 && !iswallleft && !iswallright)
            wallruncameratilt += Time.deltaTime * maxwallruncameratilt * 2;
    }
    private void MovePlayer()
    {
        if (restricted) return;
        if (activegrapple) return;
        if (exitingwall) return;
        if (exitingwallrun) return;
        if (swinging) return;
        if (state == MoveState.dashing) return;
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(OnSlope()&&exitingslope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * movespeed * 20f, ForceMode.Force);
            if(rb.velocity.y>0)
            {
                rb.AddForce(Vector3.down*80f, ForceMode.Force);
            }
        }
        if(isgrounded)
        rb.AddForce(moveDirection.normalized * movespeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * movespeed * 10f*airmultiplier, ForceMode.Force);


        if(!iswallrunning)rb.useGravity = !OnSlope();
    }
    private void SpeedControl()
    {
        if (activegrapple) return;
        if (OnSlope()&&!exitingslope)
        {
            if (rb.velocity.magnitude > movespeed)
                rb.velocity = rb.velocity.normalized * movespeed;
        }
        else
        {
            Vector3 flatvel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (flatvel.magnitude > movespeed)
            {
                Vector3 limitVel = flatvel.normalized * movespeed;
                rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
            }
        }
        if (maxYspeed != 0 && rb.velocity.y > maxYspeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYspeed, rb.velocity.z);
    }
    private void Jump()
    {
        /*
        exitingslope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        */
        if (isgrounded)
        {
            readyToJump = false;
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(slopeHit.normal * jumpForce * 0.5f);
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
            {
                rb.velocity = new Vector3(vel.x, 9, vel.z);
            }
            else if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
            }
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (!isgrounded)
        {
            readyToJump = false;
            rb.AddForce(orientation.forward * jumpForce * 1f);
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(slopeHit.normal * jumpForce * 0.5f);

            Invoke(nameof(ResetJump), jumpCooldown);

        }
        /*
        if (iswallrunning)
        {
            readyToJump = false;

            if (iswallleft && !Input.GetKey(KeyCode.D) || iswallright && !Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector2.up * jumpForce * 1.5f);
                rb.AddForce(slopeHit.normal * jumpForce * 0.5f);

            }
            if (iswallright || iswallleft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                rb.AddForce(-orientation.up * jumpForce * 1f);
            if (iswallright && Input.GetKey(KeyCode.A) )
                rb.AddForce(-orientation.right * jumpForce * 3.2f);
            if (iswallleft && Input.GetKey(KeyCode.D))
                rb.AddForce(orientation.right * jumpForce * 3.2f);
                rb.AddForce(orientation.forward * jumpForce * 1f);
            rb.velocity = Vector3.zero;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        */
    }
    private void ResetJump()
    {
        readyToJump = true;
        exitingslope=false;
    }
    #endregion

    #region Wallrun
    private void WallrunState()
    {
        upwardsrunning = Input.GetKey(KeyCode.O);
        downwardrunning = Input.GetKey(KeyCode.P);
        if ((iswallleft || iswallright) && verticalInput > 0 && AboveGround()&&!exitingwallrun)
        {
            if (!iswallrunning)
            {
                StartWallRun();
            }
            if(iswallrunning)
            {
                if (wallruntimer > 0)
                    wallruntimer -= Time.deltaTime;
                if (wallruntimer <= 0 && iswallrunning)
                {
                    exitingwallrun = true;
                    exitwallruntimer = exitwalltime;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
                WallJump();
        }
        else if(exitingwallrun)
        {
            if (iswallrunning) StopWallRun();
            if (exitwallruntimer > 0)
                exitwallruntimer -= Time.deltaTime;
            if (exitwallruntimer <= 0)
                exitingwallrun = false;
        }
        else
        {
            if (iswallrunning)
            {
                StopWallRun();
            }
        }
    }
    private void StartWallRun()
    {
        rb.useGravity = false;
        iswallrunning = true;
        wallruntimer = maxwallrunTime;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

       
    }
    private void Wallrunning()
    {
        rb.useGravity = usewallrungravity;
        Vector3 wallnormal = iswallright ? rightwallhit.normal :leftwallhit.normal;
        Vector3 wallforward = Vector3.Cross(wallnormal, transform.up);
        if ((orientation.forward - wallforward).magnitude > (orientation.forward - -wallforward).magnitude)
            wallforward = -wallforward;
        rb.AddForce(wallforward * wallrunForce, ForceMode.Force);

        if(upwardsrunning)
            rb.velocity= new Vector3(rb.velocity.x,wallclimbspeed,rb.velocity.z);
        if(downwardrunning)
            rb.velocity= new Vector3(rb.velocity.x,-wallclimbspeed,rb.velocity.z);

        if(!(iswallleft&&horizontalInput>0)||!(iswallright&&horizontalInput<0))
        rb.AddForce(-wallnormal * 100,ForceMode.Force);

        if (usewallrungravity)
            rb.AddForce(transform.up * gravitycounterforce, ForceMode.Force);
    }
    private void StopWallRun()
    {
        rb.useGravity = true;
        iswallrunning = false;
    }
    private void CheckForWall()
    {
        iswallright = Physics.Raycast(transform.position, orientation.right,out rightwallhit, 1f, wall);
        iswallleft = Physics.Raycast(transform.position, -orientation.right,out leftwallhit, 1f, wall);

        if (!iswallleft && !iswallright)
        {
            StopWallRun();
        }
        if (iswallleft||iswallright)
        {
            doubleJumpsLeft = startDoubleJumps;
        }
    }
    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minjumpheight, ground);
    }
    private void WallJump()
    {
        exitingwallrun = true;
        exitwallruntimer = exitwallruntime;
        Vector3 wallnormal = iswallright ? rightwallhit.normal : leftwallhit.normal;
        Vector3 forcetoapply = transform.up * walljumpupforce + wallnormal * walljumpsideforce;

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(forcetoapply, ForceMode.Impulse);
    }


    #endregion

    #region Climbing

    private void WallCheck()
    {
        wallfront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, wall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        bool newwall = frontWallHit.transform != lastwall || Mathf.Abs(Vector3.Angle(lastwallnormal, frontWallHit.normal)) > minwallnormalanglechange;
        if (isgrounded || (wallfront&&newwall))
        {
            climbTimer = maxClimbTime;
            climbjumpsleft = climbjumps;
        }
    }

    private void StartClimbing()
    {
        climbing = true;
        lastwall = frontWallHit.transform;
        lastwallnormal=frontWallHit.normal; 
    }
    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }
    private void StopClimbing()
    {
        climbing = false;
    }

    private void ClimbingState()
    {
        if(holding)
        {
            if (climbing) StopClimbing();
        }
        else if(wallfront&&Input.GetKey(KeyCode.W)&&wallLookAngle<maxWallLookAngle&&!exitingwall)
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        }

        else if(exitingwall)
        {
            if (climbing) StopClimbing();
            if (exitwalltimer > 0) exitwalltimer -= Time.deltaTime;
            if (exitwalltimer < 0) exitingwall = false;
        }
        else
        {
            if (climbing) StopClimbing();
        }
        if (wallfront&&Input.GetKeyDown(KeyCode.Space)&&climbjumpsleft>0)
        {
            ClimbJump();
        }
    }

    private void ClimbJump()
    {
        if (isgrounded) return;
        if (holding||exitingledge) return;
        exitingwall = true;
        exitwalltimer = exitwalltime;
        Vector3 forcetoapply = transform.up * climbJumpUPForce + frontWallHit.normal * climbJumpBackForce;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forcetoapply, ForceMode.Impulse);
        climbjumpsleft--;
    }

    #endregion

    #region Ledge

    private void LedgeDetection()
    {
        bool ledgedetected = Physics.SphereCast(transform.position, ledgeSphereCastRadius, cam.transform.forward, out ledgehit, ledgedetectionLength, ledge);
        if (!ledgedetected) return;

        float distanceToledge = Vector3.Distance(transform.position, ledgehit.transform.position);

        if (ledgehit.transform == lastLedge) return;

        if (distanceToledge < maxLedgeGrabDistance && !holding) EnterLedgeHold();
    }

    private void EnterLedgeHold()
    {
        freeze = true;
        restricted = true;
        holding = true;
        currledge = ledgehit.transform;
        lastLedge = ledgehit.transform;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }
    private void FreezeRigidbodyOnLedge()
    {
        rb.useGravity = false;
        Vector3 directionToLedge = currledge.position - transform.position;
        float distanceToLedge = Vector3.Distance(transform.position, currledge.position);

        if(distanceToLedge>1f)
        {
            if(rb.velocity.magnitude<moveToLedgeSpeed)
            rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 1000f * Time.deltaTime);
        }
        else
        {
            if (!freeze) freeze = true;
            if(unlimited) unlimited = false;
        }

        if(distanceToLedge>maxLedgeGrabDistance)
        {
            ExitLedgeHold();
        }
    }
    private void ExitLedgeHold()
    {
        exitingledge = true;
        exitledgetimer = exitledgetime;
        holding = false;
        timeOnLedge = 0; 
        restricted = false;
        freeze = false;
        rb.useGravity = true;
        StopAllCoroutines();
        Invoke(nameof(ResetLastLedge), 1f);
    }
    private void ResetLastLedge()
    {
        lastLedge = null;
    } 
    private void LedgeStateMachine()
    {
       
        bool anyInputKeyPressed = horizontalInput != 0 || verticalInput != 0;

        if(holding)
        {
            FreezeRigidbodyOnLedge();

            timeOnLedge += Time.deltaTime;
            if (timeOnLedge > minTimeOnLedge && anyInputKeyPressed) ExitLedgeHold();

            if (Input.GetKeyDown(KeyCode.Space)) LedgeJump();
        }
        else if(exitingledge)
        {
            if (exitledgetimer > 0) exitledgetimer -= Time.deltaTime;
            else exitingledge = false;
        }
    }
    private void LedgeJump()
    {
        ExitLedgeHold();
        Invoke(nameof(DelayedJumpForce), 0.05f);
    }
    private void DelayedJumpForce()
    {
  Vector3 forcetoadd = cam.transform.forward * ledgejumpforwardforce + orientation.up * ledgejumpupforce;
        rb.velocity = Vector3.zero;
        rb.AddForce(forcetoadd, ForceMode.Impulse);

    }
    #endregion

    #region Sliding

    private void StartSliding()
    {
        sliding = true;

        transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);
        rb.AddForce(Vector3.down*5f,ForceMode.Impulse);
        slideTimer = maxSlideTime;
    }
    private void Sliding()
    {
        Vector3 inputdirecton = orientation.forward*verticalInput+orientation.right*horizontalInput;

        if (!OnSlope() || rb.velocity.y>-0.1f)
        {
            rb.AddForce(inputdirecton.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        else
        {
            rb.AddForce(GetSlopeMoveDirection(inputdirecton) * slideForce, ForceMode.Force);

        }
        if (slideTimer <= 0) StopSliding();
        
    }
    private void StopSliding()
    {
        sliding = false;
        transform.localScale = new Vector3(transform.localScale.x, slidestartYScale, transform.localScale.z);
    }

    #endregion

    #region Dash

    private void Dash()
    {
        if (dashcdtimer > 0) return;
        else dashcdtimer = dashcd;
        maxYspeed = maxDashYspeed;
        dashing = true;
      //  DoFov(dashfov);
        Transform forwardT;
        if (useCameraForward)
            forwardT = cam.transform;
        else
            forwardT = orientation;

        Vector3 direction = GetDirection(forwardT);
        Vector3 forcetoapply = direction * dashforce + orientation.up * dashupwardforce;

        if (disablegravity)
            rb.useGravity = false;
        dashdelayedforcetoapply = forcetoapply;
        Invoke(nameof(DelayedDash), 0.025f);
        Invoke(nameof(ResetDash), dashduration);
    }
    private Vector3 GetDirection(Transform Forward)
    {
        float horizontalinput = Input.GetAxisRaw(""Horizontal"");
        float verticalinput = Input.GetAxisRaw(""Vertical"");
        Vector3 direction = new Vector3();

        if(allowalldirections)
            direction = Forward.forward*verticalinput+Forward.right*horizontalinput;
        else
            direction = Forward.forward;
        if(verticalinput==0&&horizontalinput==0)
        {
            direction = Forward.forward;
        }
        return direction; 
    }
    private void DelayedDash()
    {
        if (resetvel)
            rb.velocity = Vector3.zero; 
        rb.AddForce(dashdelayedforcetoapply, ForceMode.Impulse);

    }
    private void ResetDash()
    {
        dashing = false;
      //  DoFov(60);//85 
        maxYspeed = 0;
        if(disablegravity)
        rb.useGravity = true;
    }
    
    #endregion

    #region Grappling

    private void StartGrapple(int grappleindex)
    {
        if (grapplingcdtimer > 0) return;

        CancelActiveSwings();
        CancelAllGrapplsExcept(grappleindex);
        if (predictionhit[grappleindex].point!=Vector3.zero)
        {
            //grappling = true;
            grappleactive[grappleindex] = true;
            freeze = true;
            swingpoint[grappleindex] = predictionhit[grappleindex].point;
            StartCoroutine(ExecuteGrapple(grappleindex));
        }
        else
        {
            swingpoint[grappleindex] =cam.transform.position+cam.transform.forward*maxgrapplingdistance;
            Invoke(nameof(StopGrapple), grappledelaytime); 

        }
        //lr[grappleindex].enabled = true;
        lr[grappleindex].positionCount = 2;
        currentgrappleposition[grappleindex] = guntip[grappleindex].position;
       // lr[grappleindex].SetPosition(1, swingpoint[grappleindex]);
    }
    private IEnumerator ExecuteGrapple(int grappleindex)
    {
        yield return new WaitForSeconds(grappledelaytime);
        freeze = false;

        Vector3 lowestpoint = new Vector3(transform.position.x,transform.position.y-1,transform.position.z);
        float grapplePointRelativeYPos = swingpoint[grappleindex].y - lowestpoint.y;
        float highestpointonarc = grapplePointRelativeYPos + overshootYaxis;
        if(grapplePointRelativeYPos<0)
        {
            highestpointonarc = overshootYaxis;
        }
        JumpToPosition(swingpoint[grappleindex], highestpointonarc);
        StartCoroutine(StopGrapple(grappleindex, 0));
    }
    private IEnumerator StopGrapple(int grappleindex, float delay =0)
    {
        yield return new WaitForSeconds(delay);
        freeze = false;
        ResetRestrictions();
        grappleactive[grappleindex] = false;
        grapplingcdtimer = grapplingcd;
        
    }
    public void JumpToPosition(Vector3 targetposition, float trajectoryheight)
    {
        activegrapple = true;
        velocitytoset= CalculteJumpHeight(transform.position, targetposition, trajectoryheight);
        Invoke(nameof(SetVelocity), 0.1f);
    }
    private void SetVelocity()
    {
        enablemovementonnexttouch = true;
        rb.velocity = velocitytoset;
      //  DoFov(grapplefov);
    }
    public Vector3 CalculteJumpHeight(Vector3 startpoint, Vector3 endpoint, float trajectoryheight)
    {
        float gravity = Physics.gravity.y;
        float displacement = endpoint.y - startpoint.y;
        Vector3 displacementXZ = new Vector3(endpoint.x - startpoint.x, 0, endpoint.z - startpoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryheight);
        Vector3 velocityXZ= displacementXZ/(Mathf.Sqrt(-2*trajectoryheight/gravity)+Mathf.Sqrt(2*(displacement-trajectoryheight)/gravity));
        return velocityXZ + velocityY;
    }
    public void ResetRestrictions()
    {
        activegrapple = false;
        //DoFov(60);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (enablemovementonnexttouch)
        {
            enablemovementonnexttouch = false;
            ResetRestrictions();
            CancelActiveGrapples();
            //StopGrapple()
        }
    }

    #endregion

    #region Calculations
    public void DoFov(float fov)
    {
        cam.fieldOfView = fov;
    }

    #endregion

    #region Swing
    private void StartSwing(int swingindex)
    {
      
        if (predictionhit[swingindex].point == Vector3.zero) return;

        CancelActiveGrapples();
        ResetRestrictions();
        swinging = true;

        swingsactive[swingindex] = true;

        swingpoint[swingindex] = predictionhit[swingindex].point;
            joint[swingindex] = gameObject.AddComponent<SpringJoint>();
            joint[swingindex].autoConfigureConnectedAnchor = false;
            joint[swingindex].connectedAnchor = swingpoint[swingindex];

            float distancefrompoint = Vector3.Distance(transform.position, swingpoint[swingindex]);

            joint[swingindex].maxDistance = distancefrompoint * 0.8f;
            joint[swingindex].minDistance = distancefrompoint * 0.25f;

            joint[swingindex].spring = 4.5f;
            joint[swingindex].damper = 7f;
            joint[swingindex].massScale = 4.5f;

        lr[swingindex].positionCount = 2;
        currentgrappleposition[swingindex] = guntip[swingindex].position;
        
    }
    void DrawRope()
    {
        for (int i = 0; i < amountofswingpoints; i++)
        {
            if (!swingsactive[i] && !grappleactive[i])
            {
                lr[i].positionCount = 0;
            }
            else
            {
                currentgrappleposition[i] = Vector3.Lerp(currentgrappleposition[i], swingpoint[i], Time.deltaTime * 8);
                lr[i].SetPosition(0, guntip[i].position);
                lr[i].SetPosition(1, swingpoint[i]);
            }
        }
        
    }
    private void StopSwing(int swingindex)
    {
        swinging = false;
        swingsactive[swingindex]=false;
        //lr[swingindex].positionCount = 0;
        Destroy(joint[swingindex]);
    }

    private void OdmGearMovement()
    {
        if (Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * horizontalthrustforce * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * horizontalthrustforce * Time.deltaTime);
        if (Input.GetKey(KeyCode.W)) rb.AddForce(orientation.forward * forwardthrustforce * Time.deltaTime);
if (swingsactive[0] && !swingsactive[1]) pullpoint = swingpoint[0];
            if (swingsactive[1] && !swingsactive[0]) pullpoint = swingpoint[1];
            if (swingsactive[1] && swingsactive[0])
            {
                Vector3 dirgrapple = swingpoint[1] - swingpoint[0];
                pullpoint = swingpoint[0] + dirgrapple * 0.5f;
            }

        if(Input.GetKey(KeyCode.Space))
        {
            
            Vector3 directiontopoint = pullpoint - transform.position;
            rb.AddForce(directiontopoint.normalized*forwardthrustforce*Time.deltaTime);
            float distancefrompoint = Vector3.Distance(transform.position, pullpoint);
            UpdateJoints(distancefrompoint);


        }
        if(Input.GetKey(KeyCode.S))
        {
            float extendedDistancefrompoint = Vector3.Distance(transform.position, pullpoint) + extendcablespeed;

            UpdateJoints(extendedDistancefrompoint);
        }

    }
    private void UpdateJoints(float distancefrompoint)
    {
        for (int i = 0; i < joint.Count; i++)
        {
            if(joint[i]!=null)
            {
                joint[i].maxDistance = distancefrompoint * 0.8f;
                joint[i].minDistance = distancefrompoint * 0.25f;
            }
        }
    }
    private void CheckForSwingPoints()
    {
        for (int i = 0; i < amountofswingpoints; i++)
        {
            if (swingsactive[i]) { }
            else
            {
                RaycastHit spherecasthit;
                Physics.SphereCast(pointaimers[i].transform.position, predictionSphereCastRadius, pointaimers[i].transform.forward, out spherecasthit, maxswingdistance, grappleable);
                RaycastHit raycasthit;
                Physics.SphereCast(pointaimers[i].transform.position, predictionSphereCastRadius, pointaimers[i].transform.forward, out raycasthit, maxswingdistance, grappleable);

                Vector3 realhitpoint;
                if (raycasthit.point != Vector3.zero)
                    realhitpoint = raycasthit.point;
                else if (spherecasthit.point != Vector3.zero)
                    realhitpoint = spherecasthit.point;
                else
                    realhitpoint = Vector3.zero;

                if (realhitpoint != Vector3.zero)
                {
                    predictionPoint[i].gameObject.SetActive(true);
                    predictionPoint[i].position = realhitpoint;

                }
                else
                    predictionPoint[i].gameObject.SetActive(false);

                predictionhit[i] = raycasthit.point == Vector3.zero ? spherecasthit : raycasthit;
            }
        }
       
    }
    #endregion

    #region Dual
    private void ListSetup()
    {
        predictionhit = new List<RaycastHit>();
        swingpoint = new List<Vector3>();
        joint = new List<SpringJoint>();
        swingsactive = new List<bool>();
        grappleactive = new List<bool>();
        currentgrappleposition = new List<Vector3>();
        for (int i = 0; i < amountofswingpoints; i++)
        {
            predictionhit.Add(new RaycastHit());
            joint.Add(null);
            swingpoint.Add(new Vector3());
            swingsactive.Add(false);
            grappleactive.Add(false);
            currentgrappleposition.Add(new Vector3());

        }
    }
    public void CancelActiveGrapples()
    {
        StartCoroutine(StopGrapple(0));
        StartCoroutine(StopGrapple(1));
    }
    private void CancelAllGrapplsExcept(int grappleindex)
    {
        for (int i = 0; i < amountofswingpoints; i++)
        {
            if (i != grappleindex) StartCoroutine(StopGrapple(i));
        }
    }
    private void CancelActiveSwings()
    {
        StopSwing(0);
        StopSwing(1);
    }
    #endregion
}

public enum MoveState
{ 
    freeze,
    unlimited,
    swinging,
    dashing,
    Sliding,
    Wallrunning,
    Walk,
    Run,
    Crouch,
    Climbing,
    Air
}

";
    }

    public static string TPSCamera()
    {
        return @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform orientation;
    public Transform player;
    public Transform obj;
    public Rigidbody rb;
    public float rotspeed;

    public CameraStyle style;
    public Transform combatlookat;

    public GameObject Basic;
    public GameObject Combat;
    public GameObject Topdown;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewdir = player.transform.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewdir.normalized;

        if (style == CameraStyle.Basic||style==CameraStyle.Topdown)
        {
            float hinput = Input.GetAxis(""Horizontal"");
            float vinput = Input.GetAxis(""Vertical"");

            Vector3 inputdir = orientation.forward * vinput + orientation.right * hinput;
            if (inputdir != Vector3.zero)
            {
                obj.forward = Vector3.Slerp(obj.forward, inputdir.normalized, Time.deltaTime * rotspeed);
            }
        }
        else if (style == CameraStyle.Combat)
        {
            Vector3 viewdircombat = combatlookat.transform.position - new Vector3(transform.position.x, combatlookat.position.y, transform.position.z);
            orientation.forward = viewdircombat.normalized;
            obj.forward = viewdircombat.normalized; 
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyle(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyle(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCameraStyle(CameraStyle.Topdown);
    }

    private void SwitchCameraStyle(CameraStyle style)
    {
        Basic.SetActive(false);
        Combat.SetActive(false);
        Topdown.SetActive(false);

        if (style == CameraStyle.Basic) Basic.SetActive(true);
        if (style == CameraStyle.Combat) Combat.SetActive(true);
        if (style == CameraStyle.Topdown) Topdown.SetActive(true);
    }
    public enum CameraStyle
    {
        Basic,Combat,Topdown
    }
}
";
    }

    public static string NavyGrid()
    {
        return @"RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)/RGBA(0.125, 0.173, 0.216, 1.000)";
    }

    public static string DeepNavyGrid()
    {
        return @"RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)/RGBA(0.094, 0.114, 0.129, 1.000)";
    }

    public static string EmissionGrid()
    {
        return @"RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.031, 0.031, 0.031, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.000, 0.000, 0.000, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)/RGBA(0.086, 0.086, 0.086, 1.000)";
    }

    public static string RoystanToonShader()
    {
        return @"Shader ""Custom/Toon""//Roystan
{
	Properties
	{
		_Color(""Color"", Color) = (1,1,1,1)
		_MainTex(""Main Texture"", 2D) = ""white"" {}
		// Ambient light is applied uniformly to all surfaces on the object.
		[HDR]
		_AmbientColor(""Ambient Color"", Color) = (0.4,0.4,0.4,1)
		[HDR]
		_SpecularColor(""Specular Color"", Color) = (0.9,0.9,0.9,1)
		// Controls the size of the specular reflection.
		_Glossiness(""Glossiness"", Float) = 32
		[HDR]
		_RimColor(""Rim Color"", Color) = (1,1,1,1)
		_RimAmount(""Rim Amount"", Range(0, 1)) = 0.716
		// Control how smoothly the rim blends when approaching unlit
		// parts of the surface.
		_RimThreshold(""Rim Threshold"", Range(0, 1)) = 0.1		
	}
	SubShader
	{
		Pass
		{
			// Setup our pass to use Forward rendering, and only receive
			// data on the main directional light and ambient light.
			Tags
			{
				""LightMode"" = ""ForwardBase""
				""PassFlags"" = ""OnlyDirectional""
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// Compile multiple versions of this shader depending on lighting settings.
			#pragma multi_compile_fwdbase
			
			#include ""UnityCG.cginc""
			// Files below include macros and functions to assist
			// with lighting and shadows.
			#include ""Lighting.cginc""
			#include ""AutoLight.cginc""

			struct appdata
			{
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 worldNormal : NORMAL;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;	
				// Macro found in Autolight.cginc. Declares a vector4
				// into the TEXCOORD2 semantic with varying precision 
				// depending on platform target.
				SHADOW_COORDS(2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);		
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				// Defined in Autolight.cginc. Assigns the above shadow coordinate
				// by transforming the vertex from world space to shadow-map space.
				TRANSFER_SHADOW(o)
				return o;
			}
			
			float4 _Color;

			float4 _AmbientColor;

			float4 _SpecularColor;
			float _Glossiness;		

			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;	

			float4 frag (v2f i) : SV_Target
			{
				float3 normal = normalize(i.worldNormal);
				float3 viewDir = normalize(i.viewDir);

				// Lighting below is calculated using Blinn-Phong,
				// with values thresholded to creat the ""toon"" look.
				// https://en.wikipedia.org/wiki/Blinn-Phong_shading_model

				// Calculate illumination from directional light.
				// _WorldSpaceLightPos0 is a vector pointing the OPPOSITE
				// direction of the main directional light.
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				// Samples the shadow map, returning a value in the 0...1 range,
				// where 0 is in the shadow, and 1 is not.
				float shadow = SHADOW_ATTENUATION(i);
				// Partition the intensity into light and dark, smoothly interpolated
				// between the two to avoid a jagged break.
				float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);	
				// Multiply by the main directional light's intensity and color.
				float4 light = lightIntensity * _LightColor0;

				// Calculate specular reflection.
				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);
				// Multiply _Glossiness by itself to allow artist to use smaller
				// glossiness values in the inspector.
				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;				

				// Calculate rim lighting.
				float rimDot = 1 - dot(viewDir, normal);
				// We only want rim to appear on the lit side of the surface,
				// so multiply it by NdotL, raised to a power to smoothly blend it.
				float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;

				float4 sample = tex2D(_MainTex, i.uv);

				return (light + _AmbientColor + specular + rim) * _Color * sample;
			}
			ENDCG
		}

		// Shadow casting support.
        UsePass ""Legacy Shaders/VertexLit/SHADOWCASTER""
	}
}";
    }

    public static string ToonV2()
    {
        return @"Shader ""Custom/ToonWithEnvironmentLighting""
{
    Properties
    {
        _Color(""Color"", Color) = (1, 1, 1, 1)
        _MainTex(""Main Texture"", 2D) = ""white"" {}
        _AmbientColor(""Ambient Color"", Color) = (1, 1, 1, 1)
        _SpecularColor(""Specular Color"", Color) = (0.9, 0.9, 0.9, 1)
        _Glossiness(""Glossiness"", Range(1, 128)) = 32
        _RimColor(""Rim Color"", Color) = (1, 1, 1, 1)
        _RimAmount(""Rim Amount"", Range(0, 1)) = 0.716
        _RimThreshold(""Rim Threshold"", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags
        {
            ""RenderType""=""Opaque""
            ""Queue""=""Geometry""
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf ToonWithEnvLighting

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _AmbientColor;
        fixed4 _SpecularColor;
        float _Glossiness;
        fixed4 _RimColor;
        float _RimAmount;
        float _RimThreshold;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            INTERNAL_DATA
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Normal = IN.worldNormal;
            o.Specular = _SpecularColor.rgb;
            o.Gloss = _Glossiness;
            o.Alpha = _Color.a;

            // Calculate the ambient light contribution
            fixed3 ambientLight = UNITY_LIGHTMODEL_AMBIENT.rgb * _AmbientColor.rgb;

            // Calculate the reflection vector
            half3 viewDir = normalize(IN.viewDir);
            half3 reflected = reflect(-viewDir, IN.worldNormal);

            // Calculate the rim lighting
            half rimDot = 1 - dot(viewDir, IN.worldNormal);
            half rimIntensity = rimDot * pow(saturate(1 - dot(reflected, IN.worldNormal)), _RimThreshold);
            half rim = _RimAmount * _RimColor.rgb * rimIntensity;

            // Apply the ambient light, rim lighting, and color to the final pixel color
            o.Emission = ambientLight + rim + _Color.rgb;
        }
        ENDCG
    }
    FallBack ""Diffuse""
}";
    }

    public static string FlockingBoids()
    {
        return @"using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FlockingBoids : MonoBehaviour
{
    public float neighborDist = 3.0f;
    public List<GameObject> Squad;
    public GameObject target;
    public bool near;

    public void Start()
    {
       
    }
    public void Update()
    {
        if (Vector3.Distance(transform.position,target.transform.position)<1f)
        {
            near = true;
        }

        if (near == false)
        {
            foreach (GameObject go in Squad)
            {
                if (Vector3.Distance(go.transform.position, transform.position) < 5f)
                {
                    Boid_Cohesion cohesion = go.GetComponent<Boid_Cohesion>();
                    if (cohesion != null)
                    {
                        if (near == true)
                        {
                            cohesion.near = true;
                            return;
                        }
                    }
                }
            }
            transform.position += (target.transform.position - transform.position).normalized * Time.deltaTime * 5f;

        }

    }

}
";
    }

    public static string CardDropZone()
    {
        return @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropZone :MonoBehaviour, IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Slot slot;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        CardDrag d = eventData.pointerDrag.GetComponent<CardDrag>();
        if(d!= null )
        {
            d.placeholderparent = this.transform;
        }

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        CardDrag d = eventData.pointerDrag.GetComponent<CardDrag>();
        if(d!= null && d.placeholderparent==this.transform)
        {
            d.placeholderparent = d.parentToreturnTo;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;
        CardDrag d = eventData.pointerDrag.GetComponent<CardDrag>();
        if(d != null)
        {
            if(slot==d.slot)
            d.parentToreturnTo = this.transform;
        }
    }

}
";
    }

    public static string CardDrag()
    {
        return @"using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler
{
    public Transform parentToreturnTo = null;
    public Transform placeholderparent = null;
    public GameObject placeholder = null;

    public Slot slot;
    public void OnBeginDrag(PointerEventData eventData)
    {
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToreturnTo = this.transform.parent;
        placeholderparent = parentToreturnTo;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        if (placeholder.transform.parent != placeholderparent)
            placeholder.transform.SetParent(placeholderparent);
        int newSiblingIndex = placeholderparent.childCount;
        for (int i = 0; i < placeholderparent.childCount; i++)
        {
            if(this.transform.position.x<placeholderparent.GetChild(i).position.x)
            {
                newSiblingIndex = i;
                if(placeholder.transform.GetSiblingIndex()<newSiblingIndex)
                {
                    newSiblingIndex--;
                }
                break;
            }
        } 
        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToreturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeholder);
    }

   
}
public enum Slot { Weapon, Head, Chest, Legs, Feet };
";
    }

    public static string DragBase()
    {
        return @"using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragDropBase : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IDragHandler
{
    #region V1
    Vector3 offset;
    CanvasGroup canvas;
    public string tag;
    // Start is called before the first frame update
    void Awake()
    {
        canvas = this.GetOrAddComponent<CanvasGroup>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition + offset;

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        offset = transform.position - Input.mousePosition;
        canvas.alpha = 0.5f;
        canvas.blocksRaycasts = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        RaycastResult result = eventData.pointerCurrentRaycast;
        if(result.gameObject?.tag==tag)
        {
            transform.position=result.gameObject.transform.position;

        }
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
    }
    #endregion

    #region V2
    public Canvas canvas2;
    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointer = (PointerEventData)data;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointer.position, canvas2.worldCamera, out pos);

        transform.position = canvas.transform.TransformPoint(pos);
    }
    #endregion

    #region 3DV

    Vector3 mousepos;

    private Vector3 GetMousepos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }
    private void OnMouseDown()
    {
        mousepos = Input.mousePosition - GetMousepos();
    }

    private void OnMouseDrag()
    {
        transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition - mousepos);
    }

    #endregion

    #region 3DV2

    private GameObject selectedObject;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedObject == null)
            {
                RaycastHit hit = CastRay();

                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag(""drag""))
                    {
                        return;
                    }

                    selectedObject = hit.collider.gameObject;
                    Cursor.visible = false;
                }
            }
            else
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, 0f, worldPosition.z);

                selectedObject = null;
                Cursor.visible = true;
            }
        }

        if (selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, .25f, worldPosition.z);

            if (Input.GetMouseButtonDown(1))
            {
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedObject.transform.rotation.eulerAngles.x,
                    selectedObject.transform.rotation.eulerAngles.y + 90f,
                    selectedObject.transform.rotation.eulerAngles.z));
            }
        }
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

    #endregion
}
";
    }

    public static string NavmeshGridPath()
    {
        return @"using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

public class NavmeshGridPath : MonoBehaviour
{
    // Start is called before the first frame update
   
    [Header(""Grid"")]
    public Transform pointongrid;               //the point in which cell it is near
    public bool closed;                         //open = create a grid based on cell size, close = create grid based on overall size of bounds
    [Header(""Grid Settings"")]
    public Vector3Int count;                    //number of cells
    public Vector3 cellsize;                    //size of the cells
    public Vector3 overallSize;                 //for closed grid which is the overall size of the bounds
    public List<Vector3> grid;                  //list of positions
    public float yOffset;                       //offset for pentagon or bricked pattern

    [Header(""Navmesh Setting"")]
    public NavMeshAgent agent;                  //starting agent
    public NavMeshAgent targetagent;            //target agent
    public bool detectedge;                     //detect or get the nearest edge of the navmesh
    public bool allowdiagonal;                  //true = uses the navmeshpath, false= uses a straight path
    public bool generategridpath;               //start generation of the path on the grid
     Vector3 nearestpoint;                      //nearest grid cell of the pointongrid 
    List<Vector3> navpoints;                    //navmesh waypoints
    Vector3[] gridpoints;                       //positions on the grid
    List<Vector3> temppath;                     //store calculated path
    public bool showpoints;                     //show waypoint line iterations
    public bool showgridpoints;                 //show waypoint positions on grid
    public bool showtemppath;                   //show normal navmesh path
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
       
        Vector3 cellSize = Vector3.zero;
        Vector3 pos2;
        if(!closed)
         grid =Utility.GenerateGrid(transform.position,cellsize,count); 
        else
         grid = Utility.GenerateGridClosed(transform.position, overallSize, count,out cellSize); 
        Gizmos.color = Color.red;
        foreach (var item in grid)
        {
            Gizmos.DrawSphere(item, 0.125f);

        }
        if (pointongrid != null)
        {
            if (closed)
               Gizmos.DrawCube( Utility.GetNearestCell(pointongrid.position,transform.position, cellSize),cellSize);
            else
                Gizmos.DrawCube(Utility.GetNearestCell(pointongrid.position, transform.position, cellsize), cellsize);

        }

        Gizmos.color = Color.blue;
        if(detectedge)
        Gizmos.DrawCube(Utility.GetNearestNavMeshEdge(agent.transform.position),Vector3.one);
        else
            Gizmos.DrawCube(Utility.GetNearestNavMeshPoint(agent.transform.position), Vector3.one);

        Gizmos.color = Color.magenta;

        if (detectedge)
        Gizmos.DrawCube(Utility.GetNearestNavMeshEdge(targetagent.transform.position),Vector3.one);
        else
            Gizmos.DrawCube(Utility.GetNearestNavMeshPoint(targetagent.transform.position), Vector3.one);



        Gizmos.color = Color.green;

        NavMeshPath path =new NavMeshPath();
        NavMesh.CalculatePath(Utility.GetNearestNavMeshPoint(agent.transform.position), Utility.GetNearestNavMeshPoint(targetagent.transform.position), NavMesh.AllAreas, path);
        List<Vector3> gridpos= new List<Vector3>();
        if(generategridpath)
        {
            navpoints= new List<Vector3>();
            temppath= new List<Vector3>();
            if (allowdiagonal)
            {
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    Vector3[] points = Utility.IteratePositions(path.corners[i], path.corners[i + 1]);
                    for (int j = 0; j < points.Length; j++)
                    {
                        navpoints.Add(points[j]);
                        if (i == 0 && j == 0)
                        {
                            Vector3 newpos = Utility.GetNearestCell(pointongrid.position, transform.position, cellsize);
                            gridpos.Add(newpos);
                            nearestpoint = newpos;
                        }
                        else
                        {
                            Vector3 newpos = Utility.GetNearestCell(points[j], transform.position, cellsize);
                            if (!newpos.Equals(nearestpoint))
                            {
                                gridpos.Add(newpos);
                                nearestpoint = newpos;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < path.corners.Length ; i++)
                {
                    if(i==0)
                    temppath.Add( Utility.UnDiagonalDirection(path.corners[i],path.corners[i + 1]));
                    else if(i==path.corners.Length-1)
                        temppath.Add(Utility.UnDiagonalDirection(temppath[temppath.Count-1], targetagent.transform.position));
                    else
                        temppath.Add(Utility.UnDiagonalDirection(temppath[temppath.Count-1], path.corners[i + 1]));

                }

                for (int i = 0; i < temppath.Count-1; i++)
                {
                    if (i == 0)
                    {
                        Vector3[] points2 = Utility.IteratePositions(path.corners[0], temppath[0]);
                        for (int j = 0; j < points2.Length; j++)
                        {
                            navpoints.Add(points2[j]);
                            Vector3 newpos = Utility.GetNearestCell(points2[j], transform.position, cellsize);
                            if (!newpos.Equals(nearestpoint))
                            {
                                gridpos.Add(newpos);
                                nearestpoint = newpos;
                            }
                        } 
                    }
                    Vector3[] points = Utility.IteratePositions(temppath[i], temppath[i + 1]);
                    for (int j = 0; j < points.Length; j++)
                    {
                        navpoints.Add(points[j]);
                        if (i == 0 && j == 0)
                        {
                            Vector3 newpos = Utility.GetNearestCell(pointongrid.position, transform.position, cellsize);
                            gridpos.Add(newpos);
                            nearestpoint = newpos;
                        }
                        else
                        {
                            Vector3 newpos = Utility.GetNearestCell(points[j], transform.position, cellsize);
                            if (!newpos.Equals(nearestpoint))
                            {
                                gridpos.Add(newpos);
                                nearestpoint = newpos;
                            }
                        }
                    }
                   
                }
            }
            gridpoints = gridpos.ToArray();
            generategridpath = false;
           
        }
        if (showpoints)
        {
            for (int j = 0; j < navpoints.Count; j++)
            {
                Gizmos.DrawSphere(navpoints[j], 0.25f);

            }
        }
        if (showgridpoints)
        {
            for (int j = 0; j < gridpoints.Length; j++)
            {
                Gizmos.DrawSphere(gridpoints[j], 0.25f);
                 
            }
        }
        if (showtemppath)
        {
            
                Gizmos.color = Color.cyan;

                for (int i = 0; i < path.corners.Length; i++)
                {
                    Gizmos.DrawSphere(path.corners[i], 0.25f);

                }
                for (int i = 0; i < path.corners.Length-1; i++)
                {
                    Gizmos.DrawLine(path.corners[i], path.corners[i+1]);

                }
            
            /*
            int V1 = 0;
            int V2 = 1;
            Gizmos.color = Color.white;

            bool horizontal=( Mathf.Abs((path.corners[V2] - path.corners[V1]).x) >= Mathf.Abs((path.corners[V2] - path.corners[V1]).z));
            if (!horizontal)
            {
                Gizmos.DrawSphere(new Vector3(path.corners[V1].x, 0, path.corners[V2].z), 0.4f);

                Gizmos.DrawRay(path.corners[V1], new Vector3(path.corners[V1].x, 0, path.corners[V2].z) - path.corners[V1]);
            }
            else
            {
                Gizmos.DrawSphere(new Vector3(path.corners[V2].x, 0, path.corners[V1].z), 0.4f);

                Gizmos.DrawRay(path.corners[V1], new Vector3(path.corners[V2].x, 0, path.corners[V1].z) - path.corners[V1]);
            }
           */
            Gizmos.color = Color.green;

            for (int j = 0; j < temppath.Count; j++)
            {
                if(j==0)
                    Gizmos.DrawLine(path.corners[0], temppath[j]);
                else if(j<temppath.Count)
                    Gizmos.DrawLine(temppath[j-1 ], temppath[j]);
                Gizmos.DrawSphere(temppath[j], 0.25f);
                 
            }
        }
       // Gizmos.DrawCube(Utility.GetNearestCell(targetagent.transform.position, transform.position, cellsize), cellsize);


    }
}
";
    }


    #endregion

    #region Cover System
    public static Vector3 RotateAroundPivot2(Vector3 point, Vector3 pivot, Quaternion angles)
    {
        var finalpos = point - pivot;
        finalpos = angles * finalpos;
        finalpos += pivot;
        return finalpos;

    }
    public static int CompareVectors(Vector3 v1, Vector3 v2)
    {

        if (v1 == v2) return 0;

        if (Mathf.Approximately(v1.x, v2.x))
        {
            if (Mathf.Approximately(v1.z, v2.z))
                return v1.y > v2.y ? -1 : 1;
            else
                return v1.z > v2.z ? -1 : 1;
        }
        return v1.x > v2.x ? -1 : 1;
    }
    public static void InitializeScanner(Transform transform, MeshFilter[] meshfilter, ref Vector3[] line1, ref Vector3 rot, ref Vector3 direction, ref Vector3 othersidepos, DoubleSideOrientation orientation = DoubleSideOrientation.Y_nY, bool showBoundingBox = false, bool showSelectedAxis = false)
    {
        List<Vector3> verts = new List<Vector3>();
        foreach (MeshFilter mesh in meshfilter)
        {
            foreach (Vector3 Vertices in mesh.sharedMesh.vertices)
            {
                verts.Add(Vertices);
                Gizmos.color = UnityEngine.Color.black;
                Vector3 worldPos7 = transform.TransformPoint(Vertices);
                //Gizmos.DrawCube(worldPos7, Vector3.one * 2f);
            }
        }
        Vector3[] vertices = verts.ToArray();

        float[] points = new float[6];
        points[0] = vertices[0].x;
        points[1] = vertices[0].x;
        points[2] = vertices[0].y;
        points[3] = vertices[0].y;
        points[4] = vertices[0].z;
        points[5] = vertices[0].z;
        foreach (Vector3 pos in vertices)
        {
            Gizmos.color = UnityEngine.Color.black;
            if (pos.x < points[0])
            {
                points[0] = pos.x;

            }
            if (pos.x > points[1])
            {
                points[1] = pos.x;

            }
            if (pos.y < points[2])
            {
                points[2] = pos.y;

            }
            if (pos.y > points[3])
            {
                points[3] = pos.y;

            }
            if (pos.z < points[4])
            {
                points[4] = pos.z;

            }
            if (pos.z > points[5])
            {
                points[5] = pos.z;
            }
            //Vector3 worldPos = transform.TransformPoint(pos);
            // Gizmos.DrawCube(worldPos, Vector3.one * 2f);

        }
        float minX = points[0];
        float maxX = points[1];
        float minY = points[2];
        float maxY = points[3];
        float minZ = points[4];
        float maxZ = points[5];

        Gizmos.color = UnityEngine.Color.red;
        Vector3 blue = transform.TransformPoint(new Vector3(minX, minY, minZ));
        Vector3 grey = transform.TransformPoint(new Vector3(minX, minY, maxZ));
        Vector3 cyan = transform.TransformPoint(new Vector3(minX, maxY, maxZ));
        Vector3 red = transform.TransformPoint(new Vector3(maxX, maxY, maxZ));

        Vector3 clear = transform.TransformPoint(new Vector3(maxX, maxY, minZ));
        Vector3 yellow = transform.TransformPoint(new Vector3(maxX, minY, maxZ));
        Vector3 green = transform.TransformPoint(new Vector3(maxX, minY, minZ));
        Vector3 magenta = transform.TransformPoint(new Vector3(minX, maxY, minZ));
        if (showBoundingBox)
        {
            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawCube(blue, Vector3.one * 5f);
            Gizmos.color = UnityEngine.Color.red;

            Gizmos.DrawCube(red, Vector3.one * 5f);
            Gizmos.color = UnityEngine.Color.yellow;

            Gizmos.DrawCube(yellow, Vector3.one * 5f);
            Gizmos.color = UnityEngine.Color.green;

            Gizmos.DrawCube(green, Vector3.one * 5f);
            Gizmos.color = UnityEngine.Color.magenta;

            Gizmos.DrawCube(magenta, Vector3.one * 5f);
            Gizmos.color = UnityEngine.Color.cyan;

            Gizmos.DrawCube(cyan, Vector3.one * 5f);
            Gizmos.color = UnityEngine.Color.grey;

            Gizmos.DrawCube(grey, Vector3.one * 5f);
            Gizmos.color = UnityEngine.Color.clear;
            Gizmos.DrawCube(clear, Vector3.one * 5f);

            Gizmos.color = UnityEngine.Color.green;
            Gizmos.DrawLine(green, yellow);
            Gizmos.DrawLine(green, clear);
            Gizmos.DrawLine(magenta, clear);
            Gizmos.DrawLine(magenta, blue);
            Gizmos.DrawLine(green, blue);
            Gizmos.DrawLine(magenta, cyan);
            Gizmos.DrawLine(red, cyan);
            Gizmos.DrawLine(red, clear);
            Gizmos.DrawLine(red, yellow);
            Gizmos.DrawLine(grey, yellow);
            Gizmos.DrawLine(grey, cyan);
            Gizmos.DrawLine(grey, blue);

            Vector3 midpos = (green + yellow + clear + magenta + blue + cyan + red + grey) / 8;
            Gizmos.DrawLine(green, midpos);
            Gizmos.DrawLine(midpos, clear);
            Gizmos.DrawLine(magenta, midpos);
            Gizmos.DrawLine(midpos, blue);
            Gizmos.DrawLine(midpos, cyan);
            Gizmos.DrawLine(red, midpos);
            Gizmos.DrawLine(midpos, yellow);
            Gizmos.DrawLine(grey, midpos);
            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawCube(midpos, Vector3.one * 5);
        }
        #region Orientation
        switch (orientation)
        {
            case DoubleSideOrientation.Y_nY:
                {

                    // Top
                    line1[0] = clear;
                    line1[1] = magenta;
                    line1[2] = cyan;
                    line1[3] = red;
                    line1[4] = blue;

                    Gizmos.color = UnityEngine.Color.green;
                    rot = new Vector3(0, 0, 0);
                    direction = Vector3.down;
                    othersidepos = new Vector3(0, -(Vector3.Distance(red, yellow) + 20), 0);
                    break;
                }
            case DoubleSideOrientation.Z_nZ:
                {
                    // Forward
                    line1[0] = red;
                    line1[1] = cyan;
                    line1[2] = grey;
                    line1[3] = yellow;
                    line1[4] = magenta;

                    Gizmos.color = UnityEngine.Color.blue;
                    othersidepos = new Vector3(0, -(Vector3.Distance(red, yellow) + 20), 0);
                    rot = new Vector3(90, 0, 0);
                    direction = Vector3.back;
                    break;
                }
            case DoubleSideOrientation.X_nX:
                {
                    // Right
                    line1[0] = cyan;
                    line1[1] = magenta;
                    line1[2] = blue;
                    line1[3] = grey;
                    line1[4] = blue;

                    Gizmos.color = UnityEngine.Color.red;
                    othersidepos = new Vector3(0, -(Vector3.Distance(red, yellow) + 30), 0);
                    rot = new Vector3(90, 0, 90);
                    direction = Vector3.right;
                    break;
                }

        }
        if (showSelectedAxis)
        {
            Gizmos.DrawLine(line1[0], line1[1]);
            Gizmos.DrawLine(line1[1], line1[2]);
            Gizmos.DrawLine(line1[2], line1[3]);
            Gizmos.DrawLine(line1[3], line1[0]);
        }
        #endregion
    }

    public static void RepeatRaycast(Transform transform, ref List<Vector3> wallhit, ref List<Vector3> oppositewallhit, ref Checks[,] list, Vector3 pos, Vector3 direction, bool oppositeside, int x, int y, bool selfonly = false)
    {
        //Debug.Log("this2");
        RaycastHit hit5 = new RaycastHit();
        Physics.Raycast(pos + (direction * 0.05f), direction, out hit5, 1000);
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawRay(pos, direction);
        if (hit5.collider != null)
        {
            //Gizmos.color = Color.blue; 
            //Gizmos.DrawWireSphere(hit5.point,0.1f);
            // Debug.Log(hit5.collider.name);
            if (selfonly && hit5.collider == transform.GetComponent<Collider>())
            {

                if (list[x, y].starts == null)
                    list[x, y].starts = new List<Vector3>();
                if (list[x, y].ends == null)
                    list[x, y].ends = new List<Vector3>();



                if (!oppositeside)
                {
                    wallhit.Add(hit5.point);
                    wallhit.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    //wallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    Gizmos.color = UnityEngine.Color.black;
                    Gizmos.DrawWireSphere(hit5.point, 0.5f);
                    list[x, y].starts.Add(hit5.point);
                }
                else
                {
                    //oppositewallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    oppositewallhit.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    oppositewallhit.Add(hit5.point);
                    Gizmos.color = UnityEngine.Color.cyan;
                    Gizmos.DrawWireSphere(hit5.point, 0.5f);
                    list[x, y].ends.Insert(0, hit5.point);

                }

                RepeatRaycast(transform, ref wallhit, ref oppositewallhit, ref list, hit5.point, direction, oppositeside, x, y, selfonly);

            }

            else if (selfonly == false)
            {



                //Gizmos.color = Color.black;
                if (!oppositeside)
                {
                    //wallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    wallhit.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    wallhit.Add(hit5.point);
                    Gizmos.color = UnityEngine.Color.black;
                    Gizmos.DrawWireSphere(hit5.point, 0.5f);
                    list[x, y].starts.Add(hit5.point);
                }
                else
                {
                    //oppositewallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    oppositewallhit.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    oppositewallhit.Add(hit5.point);
                    Gizmos.color = UnityEngine.Color.cyan;
                    Gizmos.DrawWireSphere(hit5.point, 0.5f);
                    list[x, y].ends.Insert(0, hit5.point);

                }

                RepeatRaycast(transform, ref wallhit, ref oppositewallhit, ref list, hit5.point, direction, oppositeside, x, y, selfonly);

            }
            else
            {
                //Debug.Log("this2");



                RepeatRaycast(transform, ref wallhit, ref oppositewallhit, ref list, hit5.point, direction, oppositeside, x, y, selfonly);


            }
        }
    }

    public static bool CheckGrid(int x, int y, int z, Vector3 size, Vector3[,,] nativemidgrid, Transform transform)
    {
        bool U = Physics.OverlapBox(nativemidgrid[x, y, z + 1], size / 2, transform.rotation).Length > 0;
        /*
        bool F = Physics.OverlapBox(nativemidgrid[x + 1, y, z], size / 2, transform.rotation).Length > 0;
        bool B = Physics.OverlapBox(nativemidgrid[x - 1, y, z], size / 2, transform.rotation).Length > 0;
        bool R = Physics.OverlapBox(nativemidgrid[x , y+1, z], size / 2, transform.rotation).Length > 0;
        bool L = Physics.OverlapBox(nativemidgrid[x , y-1, z], size / 2, transform.rotation).Length > 0;
        bool U2 = Physics.OverlapBox(nativemidgrid[x , y, z+2], size / 2, transform.rotation).Length > 0;
        bool D = Physics.OverlapBox(nativemidgrid[x , y, z-1], size / 2, transform.rotation).Length > 0;
        bool FU = Physics.OverlapBox(nativemidgrid[x + 1, y, z+1], size / 2, transform.rotation).Length > 0;
        bool FD = Physics.OverlapBox(nativemidgrid[x + 1, y, z-1], size / 2, transform.rotation).Length > 0;
        bool FL = Physics.OverlapBox(nativemidgrid[x + 1, y-1, z], size / 2, transform.rotation).Length > 0;
        bool FLU = Physics.OverlapBox(nativemidgrid[x + 1, y-1, z+1], size / 2, transform.rotation).Length > 0;
        bool FLD = Physics.OverlapBox(nativemidgrid[x + 1, y-1, z-1], size / 2, transform.rotation).Length > 0;
        bool FR = Physics.OverlapBox(nativemidgrid[x + 1, y+1, z], size / 2, transform.rotation).Length > 0;
        bool FRU = Physics.OverlapBox(nativemidgrid[x + 1, y+1, z+1], size / 2, transform.rotation).Length > 0;
        bool FRD = Physics.OverlapBox(nativemidgrid[x + 1, y+1, z-1], size / 2, transform.rotation).Length > 0;
        bool BU = Physics.OverlapBox(nativemidgrid[x - 1, y, z+1], size / 2, transform.rotation).Length > 0;
        bool BD = Physics.OverlapBox(nativemidgrid[x - 1, y, z-1], size / 2, transform.rotation).Length > 0;
        bool BL = Physics.OverlapBox(nativemidgrid[x - 1, y-1, z], size / 2, transform.rotation).Length > 0;
        bool BLU = Physics.OverlapBox(nativemidgrid[x + 1, y-1, z+1], size / 2, transform.rotation).Length > 0;
        bool BLD = Physics.OverlapBox(nativemidgrid[x + 1, y-1, z-1], size / 2, transform.rotation).Length > 0;
        bool BR = Physics.OverlapBox(nativemidgrid[x - 1, y+1, z], size / 2, transform.rotation).Length > 0;
        bool BRU = Physics.OverlapBox(nativemidgrid[x - 1, y+1, z+1], size / 2, transform.rotation).Length > 0;
        bool BRD = Physics.OverlapBox(nativemidgrid[x + 1, y+1, z-1], size / 2, transform.rotation).Length > 0;
        bool LU = Physics.OverlapBox(nativemidgrid[x , y-1, z+1], size / 2, transform.rotation).Length > 0;
        bool LD = Physics.OverlapBox(nativemidgrid[x, y - 1, z - 1], size / 2, transform.rotation).Length > 0;
        bool RU = Physics.OverlapBox(nativemidgrid[x, y + 1, z + 1], size / 2, transform.rotation).Length > 0;
        bool RD = Physics.OverlapBox(nativemidgrid[x, y + 1, z - 1], size / 2, transform.rotation).Length > 0;
        */
        bool C = Physics.OverlapBox(nativemidgrid[x, y, z], size / 2, transform.rotation).Length > 0;

        if ((C && !U))//||(C&&U&&D&&L&&R&&!U2) ||(C&&U&&D&&L&&!R&&!U2) )
        {

            return true;
        }

        return false;
    }

    public static void ClearMultipleList(params List<Vector3>[] list)
    {
        foreach (var item in list)
        {
            item.Clear();
        }
    }




    #endregion

    #region Save/load

    #region XML Saving

    /*
    ItemDatabase itemdb = new ItemDatabase();
        string filePath = Path.Combine(Application.dataPath, "XML Files/item_data.xml");

        // Save items
        XMLDataManager<ItemDatabase>.SaveItems(itemdb, filePath);

        // Load items
        ItemDatabase loadedData = XMLDataManager<ItemDatabase>.LoadItems(filePath);
    */
    public static void SaveItems<T>(T data, string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        FileStream stream = new FileStream(filePath, FileMode.Create);
        serializer.Serialize(stream, data);
        stream.Close();
    }

    public static T LoadItems<T>(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        FileStream stream = new FileStream(filePath, FileMode.Open);
        T data = (T)serializer.Deserialize(stream);
        stream.Close();
        return data;
    }

    #endregion

    #region PlayerPref

    public static void SaveData<T>(T data, string key)
    {
        string serializedData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, serializedData);
        PlayerPrefs.Save();
    }

    public static object LoadData(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            object serializedData = PlayerPrefs.GetString(key);
            return serializedData;
        }
        else
            return null;
    }

    #endregion

    #region BinaryFormatter

    public static bool Save(string savename, object savedata)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (!Directory.Exists(Application.persistentDataPath + "/saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }
        string path = Application.persistentDataPath + "/saves/" + savename + ".save";
        FileStream stream = File.Create(path);
        formatter.Serialize(stream, savedata);
        stream.Close();
        return true;
    }

    public static object Load(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(path, FileMode.Open);
        try
        {
            object save = formatter.Deserialize(stream);
            stream.Close();
            return save;
        }
        catch
        {
            Debug.Log("No save");
            stream.Close();
            return null;
        }
    }

    #endregion

    #region JsonUtility

    public static void SaveJsonData<T>(T data, string savePath)
    {
        string jsonData = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(savePath, jsonData);
    }

    public static T LoadJsonData<T>(string savePath)
    {
        if (System.IO.File.Exists(savePath))
        {
            string jsonData = System.IO.File.ReadAllText(savePath);
            T data = JsonUtility.FromJson<T>(jsonData);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found at path: " + savePath);
            return default(T);
        }
    }

    #endregion

    #endregion

    /*
    //countdown before add, temp is timed
    public static float AddTimedTemporaryEffectsInstantly()
    {

    }
    public static float AddTimedPermanentEffectsInstantly()
    {

    }
    //instant, temp is timed
    public static float TemporaryEffectsInstantly()
    {

    }
     public static float PermanentEffectsInstantly()
    {

    }
    public static float TemporaryEffectsInstantly()
    {

    }
     public static float PermanentEffectsInstantly()
    {

    }
    */

}

public enum AnchorPreset
{
    TopLeft,
    TopCenter,
    TopRight,
    MiddleLeft,
    MiddleCenter,
    MiddleRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}

public enum Orientation
{
    X,
    Y,
    Z,
    N_X,
    N_Y,
    N_Z
}
