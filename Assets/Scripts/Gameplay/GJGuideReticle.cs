using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GJRotatingReticle))]
public class GJGuideReticle : MonoBehaviour {

    public enum Type { NOT_SET, LEFT, RIGHT };

    static GJGuideReticle rightReticle;
    static GJGuideReticle leftReticle;

    public static Queue<GameObject> rReticleQ;
    public static Queue<GameObject> lReticleQ;

    // [SerializeField]
    public GameObject target;
    Vector3 oldSpawnPos; // The position of the target on the time it was set.
    Vector3 newSpawnPos;
    public float reticleOffset;
    public float indicationRange;
    public bool drawLines;
    public Type reticleType;
    public bool isOnScreen; // bool that is whether the reticle is visible on the screen or not @note: this is untested for VR

    public Transform player;
    // GameSettings.killRange .. // The range at which the target should be shot.

    void Awake() {
        rightReticle = GameObject.Find("GJGuideReticleR").GetComponent<GJGuideReticle>();
        leftReticle = GameObject.Find("GJGuideReticleL").GetComponent<GJGuideReticle>();

        rReticleQ = new Queue<GameObject>();
        lReticleQ = new Queue<GameObject>();
    }

    void Start() {
        if (!player) {
            Debug.LogWarning("player is not specified, will use Camera.main.transform instead, this may cause innacuracies if the VR Headset is not stationary");
            player = Camera.main.transform;
        }

    }

	void Update () {
        // Attempt to acquire a target
        if (target == null) AcquireTarget();
        if (target == null) {
            GetComponent<RectTransform>().localScale = Vector3.zero;
            GetComponent<RectTransform>().position = Vector3.zero;
            return;
        } else {
            GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }

        Vector3 killPos = player.position + (target.transform.position - player.position).normalized * GJLevel.instance.killRange;
        float currentDist = Vector3.Distance(killPos, target.transform.position);

        // If target is still out of range...
        if (currentDist > 0) {
            // Calculate lerp value using the newSpawnPos as a baseline for the lerp.
            float totalDist = Vector3.Distance(newSpawnPos, killPos);
            float t = 1f - (currentDist / totalDist);

            Vector3 retOffset = (player.transform.position - target.transform.position).normalized * reticleOffset; 
            GetComponent<RectTransform>().position = Vector3.Slerp(oldSpawnPos, killPos + retOffset, t);
            
            // GetComponent<RectTransform>().localScale = Vector3.Lerp(new Vector3(1,1,1) * 0.2f, new Vector3(1, 1, 1), t);
            // GetComponent<Renderer>().material.color = Color.Lerp(Color.green, Color.red, t);
        }

        float shootDist = Vector3.Distance(GetComponent<RectTransform>().position, target.transform.position);
        if (shootDist <= indicationRange) {
            Renderer tmp = target.GetComponent<Renderer>();
            if (tmp)
                target.GetComponent<Renderer>().material.color = Color.red;
        }

        isOnScreen = OnScreen();
        if (!drawLines) return;
        Debug.DrawLine(player.position, killPos, Color.red);
        Debug.DrawLine(newSpawnPos, killPos, Color.blue);
	}

    private void SetTarget(GameObject _target) {
        if (_target == null) return;
        target = _target;
        oldSpawnPos = GetComponent<RectTransform>().position;
        newSpawnPos = target.transform.position;
    }

    void AcquireTarget() {
        switch(reticleType) {
            case Type.LEFT:
                if (lReticleQ.Count > 0)
                    SetTarget(lReticleQ.Dequeue());
                break;
            case Type.RIGHT:
                if (rReticleQ.Count > 0)
                    SetTarget(rReticleQ.Dequeue());
                break;
            default:
                Debug.LogWarning("A GJGuideReticle type has not been set and will be disabled");
                gameObject.SetActive(false);
                break;
        }
    }

    bool OnScreen() {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }
}
