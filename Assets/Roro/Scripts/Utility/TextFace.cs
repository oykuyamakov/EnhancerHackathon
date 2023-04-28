using UnityEngine;

namespace Utility
{
    public class TextFace : MonoBehaviour
    {
        void Update () {
            transform.rotation = Quaternion.LookRotation( transform.position - Camera.main.transform.position );
        }
    }
}
