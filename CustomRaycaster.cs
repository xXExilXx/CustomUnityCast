namespace Exil_S.CustomScripts
{
    using UnityEngine;

    public class CustomRaycaster
    {
        /// <summary>
        /// Raycast will check if a object has been hit
        /// </summary>
        /// <param name="origin">Origen of the Ray</param>
        /// <param name="direction">Direction of the Ray</param>
        /// <returns>Returns if hit</returns>
        public static bool Raycast(Vector3 origin, Vector3 direction)
        {
            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
            float closestDistance = Mathf.Infinity;
            bool hasHit = false;
            foreach (var obj in objects)
            {
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null && RaycastAgainstCollider(collider, origin, direction, out Vector3 hitPoint, out float hitDistance, out Vector3 hitNormal, Mathf.Infinity))
                {
                    if (hitDistance < closestDistance) // is this gonna be the closest hit?
                    {
                        closestDistance = hitDistance;
                        if (obj.layer == 0) // check if the object layer is the target layer
                        {
                            hasHit = true;
                        }
                    }
                }
            }
            return hasHit; // returning if we found a hit
        }

        /// <summary>
        /// Raycast will check if a object has been hit
        /// </summary>
        /// <param name="origin">Origen of the Ray</param>
        /// <param name="direction">Direction of the Ray</param>
        /// <param name="hitInfo">Hit Info if Hit</param>
        /// <returns>Returns if hit</returns>
        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
        {
            hitInfo = new RaycastHit();
            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
            float closestDistance = Mathf.Infinity;
            bool hasHit = false;
            foreach (var obj in objects)
            {
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null && RaycastAgainstCollider(collider, origin, direction, out Vector3 hitPoint, out float hitDistance, out Vector3 hitNormal, Mathf.Infinity))
                {
                    if (hitDistance < closestDistance) // is this gonna be the closest hit?
                    {
                        closestDistance = hitDistance;
                        hitInfo.hitObject = obj;
                        hitInfo.hitPoint = hitPoint;
                        hitInfo.hitDistance = hitDistance;
                        hitInfo.hitNormal = hitNormal;
                        if (obj.layer == 0) // check if the object layer is the target layer
                        {
                            hasHit = true;
                        }
                    }
                }
            }
            return hasHit; // returning if we found a hit
        }

        /// <summary>
        /// Raycast will check if a object has been hit
        /// </summary>
        /// <param name="origin">Origen of the Ray</param>
        /// <param name="direction">Direction of the Ray</param>
        /// <param name="hitInfo">Hit Info if Hit</param>
        /// <param name="layer">Layer to be hit</param>
        /// <returns>Returns if hit</returns>
        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, int layer)
        {
            hitInfo = new RaycastHit();
            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
            float closestDistance = Mathf.Infinity;
            bool hasHit = false;
            foreach (var obj in objects)
            {
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null && RaycastAgainstCollider(collider, origin, direction, out Vector3 hitPoint, out float hitDistance, out Vector3 hitNormal, Mathf.Infinity))
                {
                    if (hitDistance < closestDistance) // is this gonna be the closest hit?
                    {
                        closestDistance = hitDistance;
                        hitInfo.hitObject = obj;
                        hitInfo.hitPoint = hitPoint;
                        hitInfo.hitDistance = hitDistance;
                        hitInfo.hitNormal = hitNormal;
                        if (obj.layer == layer) // check if the object layer is the target layer
                        {
                            hasHit = true;
                        }
                    }
                }
            }
            return hasHit; // returning if we found a hit
        }

        /// <summary>
        /// Raycast will check if a object has been hit
        /// </summary>
        /// <param name="origin">Origen of the Ray</param>
        /// <param name="direction">Direction of the Ray</param>
        /// <param name="hitInfo">Hit Info if Hit</param>
        /// <param name="layer">Layer to be hit</param>
        /// <param name="maxDistance">The Maximal Distance the Ray can Go</param>
        /// <returns>Returns if hit</returns>
        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, int layer = 0, float maxDistance = Mathf.Infinity)
        {
            hitInfo = new RaycastHit();
            GameObject[] objects = GameObject.FindObjectsOfType<GameObject>();
            float closestDistance = maxDistance;
            bool hasHit = false;
            foreach (var obj in objects)
            {
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null && RaycastAgainstCollider(collider, origin, direction, out Vector3 hitPoint, out float hitDistance, out Vector3 hitNormal, maxDistance))
                {
                    if (hitDistance < closestDistance) // is this gonna be the closest hit?
                    {
                        closestDistance = hitDistance;
                        hitInfo.hitObject = obj;
                        hitInfo.hitPoint = hitPoint;
                        hitInfo.hitDistance = hitDistance;
                        hitInfo.hitNormal = hitNormal;
                        if(obj.layer == layer) // check if the object layer is the target layer
                        {
                            hasHit = true;
                        }
                    }
                }
            }
            return hasHit; // returning if we found a hit
        }
        private static bool RaycastAgainstCollider(Collider collider, Vector3 origin, Vector3 direction, out Vector3 hitPoint, out float hitDistance, out Vector3 hitNormal, float maxDistance)
        {
            hitPoint = Vector3.zero;
            hitDistance = maxDistance;
            hitNormal = Vector3.zero;
            switch (collider)
            {
                case SphereCollider:
                    return RaycastSphere((SphereCollider)collider, origin, direction, out hitPoint, out hitDistance, out hitNormal);
                case BoxCollider:
                    return RaycastBox((BoxCollider)collider, origin, direction, out hitPoint, out hitDistance, out hitNormal);
                case CapsuleCollider:
                    return RaycastCapsule((CapsuleCollider)collider, origin, direction, out hitPoint, out hitDistance, out hitNormal);
                case MeshCollider:
                    return RaycastMesh((MeshCollider)collider, origin, direction, out hitPoint, out hitDistance, out hitNormal);
                default:
                    return false; // no collider, so no hit
            }
        }

        private static bool RaycastSphere(SphereCollider sphereCollider, Vector3 origin, Vector3 direction, out Vector3 hitPoint, out float hitDistance, out Vector3 hitNormal)
        {
            hitDistance = float.MaxValue;
            Vector3 sphereCenter = sphereCollider.transform.position + sphereCollider.center;
            float radius = sphereCollider.radius * Mathf.Max(sphereCollider.transform.localScale.x, sphereCollider.transform.localScale.y, sphereCollider.transform.localScale.z);
            Vector3 offset = origin - sphereCenter;
            float a = Vector3.Dot(direction, direction);
            float b = 2.0f * Vector3.Dot(offset, direction);
            float c = Vector3.Dot(offset, offset) - radius * radius;
            float discriminant = b * b - 4 * a * c;
            if (discriminant > 0)
            {
                float t1 = (-b - Mathf.Sqrt(discriminant)) / (2.0f * a);
                float t2 = (-b + Mathf.Sqrt(discriminant)) / (2.0f * a);
                if (t1 > 0 && t1 < hitDistance)
                {
                    hitDistance = t1;
                    hitPoint = origin + t1 * direction;
                    hitNormal = (hitPoint - sphereCenter).normalized; // normal at hit point, I hope this makes sense
                    return true;
                }
                else if (t2 > 0 && t2 < hitDistance)
                {
                    hitDistance = t2;
                    hitPoint = origin + t2 * direction;
                    hitNormal = (hitPoint - sphereCenter).normalized;
                    return true;
                }
            }
            hitPoint = Vector3.zero;
            hitNormal = Vector3.zero;
            return false; // no hit, guess that didnt work out
        }
        private static bool RaycastBox(BoxCollider boxCollider, Vector3 origin, Vector3 direction, out Vector3 hitPoint, out float hitDistance, out Vector3 hitNormal)
        {
            hitPoint = Vector3.zero;
            hitDistance = float.MaxValue;
            hitNormal = Vector3.zero;
            Vector3 boxCenter = boxCollider.transform.position + boxCollider.center;
            Vector3 halfExtents = boxCollider.size * 0.5f;
            Matrix4x4 localToWorld = Matrix4x4.TRS(boxCenter, boxCollider.transform.rotation, Vector3.one).inverse;
            Vector3 localOrigin = localToWorld.MultiplyPoint3x4(origin);
            Vector3 localDirection = localToWorld.MultiplyVector(direction).normalized;
            float[] tMin = new float[3];
            float[] tMax = new float[3];
            for (int i = 0; i < 3; i++)
            {
                if (Mathf.Abs(localDirection[i]) < Mathf.Epsilon)
                {
                    if (localOrigin[i] < -halfExtents[i] || localOrigin[i] > halfExtents[i]) return false; // uh oh, out of bounds
                    tMin[i] = -Mathf.Infinity;
                    tMax[i] = Mathf.Infinity;
                }
                else
                {
                    tMin[i] = (-halfExtents[i] - localOrigin[i]) / localDirection[i];
                    tMax[i] = (halfExtents[i] - localOrigin[i]) / localDirection[i];
                }
            }
            float tNear = Mathf.Max(Mathf.Max(tMin[0], tMin[1]), tMin[2]);
            float tFar = Mathf.Min(Mathf.Min(tMax[0], tMax[1]), tMax[2]);
            if (tNear > tFar || tFar < 0) return false; // did we really miss it?
            hitDistance = tNear;
            hitPoint = origin + direction * hitDistance;
            hitNormal = CalculateBoxNormal(localOrigin + localDirection * hitDistance, halfExtents);
            return true; // yay, we hit something
        }
        private static Vector3 CalculateBoxNormal(Vector3 point, Vector3 halfExtents)
        {
            Vector3 normal = Vector3.zero;
            if (Mathf.Abs(point.x + halfExtents.x) < Mathf.Epsilon) normal.x = -1; // left face
            else if (Mathf.Abs(point.x - halfExtents.x) < Mathf.Epsilon) normal.x = 1; // right face
            if (Mathf.Abs(point.y + halfExtents.y) < Mathf.Epsilon) normal.y = -1; // bottom face
            else if (Mathf.Abs(point.y - halfExtents.y) < Mathf.Epsilon) normal.y = 1; // top face
            if (Mathf.Abs(point.z + halfExtents.z) < Mathf.Epsilon) normal.z = -1; // back face
            else if (Mathf.Abs(point.z - halfExtents.z) < Mathf.Epsilon) normal.z = 1; // front face
            return normal.normalized; // is this the right normal?
        }
        private static bool RaycastCapsule(CapsuleCollider capsuleCollider, Vector3 origin, Vector3 direction, out Vector3 hitPoint, out float hitDistance, out Vector3 hitNormal)
        {
            hitPoint = Vector3.zero;
            hitDistance = float.MaxValue;
            hitNormal = Vector3.zero;
            Vector3 point1 = capsuleCollider.transform.position + capsuleCollider.center + (capsuleCollider.direction == 0 ? Vector3.up : Vector3.forward) * (capsuleCollider.height * 0.5f - capsuleCollider.radius);
            Vector3 point2 = capsuleCollider.transform.position + capsuleCollider.center - (capsuleCollider.direction == 0 ? Vector3.up : Vector3.forward) * (capsuleCollider.height * 0.5f - capsuleCollider.radius);
            float radius = capsuleCollider.radius;
            // checking hits against both spheres at the ends of the capsule
            if (RaycastSphere(new SphereCollider { radius = radius, center = point1 }, origin, direction, out Vector3 hitSphere1, out float hitDistance1, out Vector3 normalSphere1))
            {
                if (hitDistance1 < hitDistance) // closer hit, nice!
                {
                    hitDistance = hitDistance1;
                    hitPoint = hitSphere1;
                    hitNormal = normalSphere1;
                }
            }
            if (RaycastSphere(new SphereCollider { radius = radius, center = point2 }, origin, direction, out Vector3 hitSphere2, out float hitDistance2, out Vector3 normalSphere2))
            {
                if (hitDistance2 < hitDistance) // another hit, could it be closer?
                {
                    hitDistance = hitDistance2;
                    hitPoint = hitSphere2;
                    hitNormal = normalSphere2;
                }
            }
            Vector3 directionToCenter = (point2 - point1).normalized;
            float segmentLength = Vector3.Distance(point1, point2);
            float closestPoint = Mathf.Clamp(Vector3.Dot(origin - point1, directionToCenter) / segmentLength, 0, 1);
            Vector3 closestPointOnSegment = point1 + directionToCenter * closestPoint;
            float distanceToCylinder = Vector3.Distance(closestPointOnSegment, origin) - radius;
            if (distanceToCylinder < 0) // hit with the cylinder part?
            {
                hitDistance = Mathf.Min(hitDistance, 0);
                hitPoint = origin; // origin should be the hit point in this case?
                hitNormal = (origin - closestPointOnSegment).normalized; // normal from the hit point?
                return true;
            }
            return false; // no hit
        }
        private static bool RaycastMesh(MeshCollider meshCollider, Vector3 origin, Vector3 direction, out Vector3 hitPoint, out float hitDistance, out Vector3 hitNormal)
        {
            hitPoint = Vector3.zero;
            hitDistance = float.MaxValue;
            hitNormal = Vector3.zero;
            Mesh mesh = meshCollider.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            for (int i = 0; i < triangles.Length; i += 3) // iterating through triangles
            {
                Vector3 v0 = meshCollider.transform.TransformPoint(vertices[triangles[i]]);
                Vector3 v1 = meshCollider.transform.TransformPoint(vertices[triangles[i + 1]]);
                Vector3 v2 = meshCollider.transform.TransformPoint(vertices[triangles[i + 2]]);

                if (RaycastTriangle(origin, direction, v0, v1, v2, out Vector3 hit, out float dist, out Vector3 normal))
                {
                    if (dist < hitDistance) // closest hit check
                    {
                        hitDistance = dist;
                        hitPoint = hit;
                        hitNormal = normal;
                    }
                }
            }
            return hitDistance < float.MaxValue; // did we hit something?
        }
        private static bool RaycastTriangle(Vector3 o, Vector3 d, Vector3 v0, Vector3 v1, Vector3 v2, out Vector3 hp, out float hd, out Vector3 hn)
        {
            hp = Vector3.zero;
            hd = float.MaxValue;
            hn = Vector3.zero;
            Vector3 e1 = v1 - v0;
            Vector3 e2 = v2 - v0;
            Vector3 h = Vector3.Cross(d, e2);
            float a = Vector3.Dot(e1, h);
            if (a > -Mathf.Epsilon && a < Mathf.Epsilon) return false; // no intersection? I hope not
            float f = 1.0f / a;
            Vector3 s = o - v0;
            float u = f * Vector3.Dot(s, h);
            if (u < 0.0f || u > 1.0f) return false; // outside triangle, maybe I messed up
            Vector3 q = Vector3.Cross(s, e1);
            float v = f * Vector3.Dot(d, q);
            if (v < 0.0f || u + v > 1.0f) return false; // still outside? whats happening?
            float t = f * Vector3.Dot(e2, q);
            if (t > Mathf.Epsilon) // hit detected, phew!
            {
                hd = t;
                hp = o + d * t;
                hn = Vector3.Cross(e1, e2).normalized; // normal, lets see if this is right
                return true;
            }
            return false; // no hit
        }
    }
    public struct RaycastHit
    {
        public Vector3 hitPoint;
        public GameObject hitObject;
        public float hitDistance;
        public Vector3 hitNormal;

        public Rigidbody rigidbody
        {
            get
            {
                return hitObject.GetComponent<Rigidbody>();
            }
        }

        public Collider collider
        {
            get
            {
                return hitObject.GetComponent<Collider>();
            }
        }
    }
}