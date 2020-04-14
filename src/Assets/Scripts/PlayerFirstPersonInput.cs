using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstPersonInput : MonoBehaviour
{
    //Object that manages unit movement
    UnitController unitController;
    //Object that manages weapons
    WeaponController weaponController;
    //Object that manages camera
    FirstPersonCamera firstPersonCamera;

    //C is for no movement
    enum Direction { C, N, NE, E, SE, S, SW, W, NW };

    //Direction mapping to vectors
    Dictionary<Direction, Vector3> directions = new Dictionary<Direction, Vector3>
    {
        [Direction.C] = Vector3.zero,
        [Direction.N] = Vector3.forward,
        [Direction.NE] = (Vector3.forward + Vector3.right) / 1.41f,
        [Direction.E] = Vector3.right,
        [Direction.SE] = (-Vector3.forward + Vector3.right) / 1.41f,
        [Direction.S] = -Vector3.forward,
        [Direction.SW] = (-Vector3.forward - Vector3.right) / 1.41f,
        [Direction.W] = -Vector3.right,
        [Direction.NW] = (Vector3.forward - Vector3.right) / 1.41f,
    };
    Direction dir = Direction.C;
    int stepableLayer;

    Vector3 mouseLastPos;
    [SerializeField]
    float verticalSensitivity = 10f;
    [SerializeField]
    float horizontalSensitivity = 10f;
    [SerializeField]
    float recoilSensitivity = 10f;
    [SerializeField]
    float recoilTime = .1f;

    float focusSensibility = 1;

    private void Start()
    {
        Cursor.visible = false;
        firstPersonCamera = GameObject.FindObjectOfType<FirstPersonCamera>();
        unitController = GetComponentInChildren<UnitController>();
        weaponController = GetComponentInChildren<WeaponController>();
        stepableLayer = LayerMask.GetMask("Stepable");
        mouseLastPos = Input.mousePosition;
    }

    //U: Check user inputs and execute player actions, only works if player control is active
    void Update()
    {
        {
            LookAt();
            Move();
            WeaponAction();
            Lantern();
        }
    }

    //U: Returns the direction the player should move depending on user input
    private Direction GetDirectionInput()
    {
        Direction result = Direction.C;

        //U: Checks user input
        bool upKey = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W));
        bool rightKey = (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D));
        bool downKey = (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S));
        bool leftKey = (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A));

        //U: If two incompatible keys are pressed they neutralize each other
        if (downKey && upKey)
        {
            downKey = false;
            leftKey = false;
        }
        if (rightKey && leftKey)
        {
            rightKey = false;
            leftKey = false;
        }

        //U: First check for diagonals moves, then for axis movement
        if (upKey && rightKey)
        {
            result = Direction.NE;
        }
        else if (downKey && rightKey)
        {
            result = Direction.SE;
        }
        else if (downKey && leftKey)
        {
            result = Direction.SW;
        }
        else if (upKey && leftKey)
        {
            result = Direction.NW;
        }
        else if (upKey)
        {
            result = Direction.N;
        }
        else if (rightKey)
        {
            result = Direction.E;
        }
        else if (downKey)
        {
            result = Direction.S;
        }
        else if (leftKey)
        {
            result = Direction.W;
        }

        return result;
    }

    //U: Moves the player around
    private void Move()
    {
        dir = GetDirectionInput();
        Vector3 worldDir = transform.localToWorldMatrix * directions[dir];
        unitController.SetMovDir(worldDir);

        //Check if player crawls
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            unitController.Crawl(true);
        else
            unitController.Crawl(false);

        //Trys to jump
        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log("Jump: " + unitController.TryJump());
    }

    //This objects are created here to avoid instantiating every frame

    //U: Rotates the player towards the aim
    private void LookAt()
    {
        //TODO: arreglar este monstruo
        var delta = (Input.mousePosition - mouseLastPos) * 4f / Screen.width;
        Vector3 forwardPoint = transform.position + transform.forward;
        Vector3 horizontalDelta = transform.right * Input.GetAxisRaw("Mouse X") * horizontalSensitivity * 0.01f * focusSensibility;
        float cyclicIntensity = (1f + 0.6f *  Mathf.Sin(Time.time * 2f)) * (1.25f - unitController.GetCrawlInterpol());
        float horizontalSeed =  0.5f * (Time.time  - 0.5f) * (1f + 0.8f * Mathf.Sin(-Time.time) * Mathf.Cos(Time.time));
        Vector3 horizontalNoise = cyclicIntensity * transform.right * Mathf.Sin(horizontalSeed) * 0.00015f;
        unitController.LookAt(forwardPoint + horizontalDelta + horizontalNoise);
        float verticalSeed = Time.time * 0.5f * (1.2f + 0.85f * Mathf.Sin(Time.time) * Mathf.Cos(Time.time));
        float verticalDelta = Input.GetAxisRaw("Mouse Y") * verticalSensitivity * focusSensibility;
        float verticalNoise = cyclicIntensity * Mathf.Sin(verticalSeed) * 0.008f;
        weaponController.RotateUpDown(verticalDelta + verticalNoise);
        mouseLastPos = Input.mousePosition;
        


    }

    private void Lantern()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            bool on = unitController.LanternIsOn();
            Debug.Log("PlayerInput: Lantern(" + on + ")");
            unitController.LanternOn(!on);
        }
    }

    //U: Shoots or change weapon if commanded
    private void WeaponAction()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponController.SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponController.SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weaponController.SwitchWeapon(2);
        }
        if (Input.GetMouseButton(0))
        {
           StartCoroutine(RecoilCo(weaponController.TryAction(), recoilTime));
        }
        if (Input.GetMouseButtonDown(1))
        {
            firstPersonCamera.Focus(weaponController.GetFocusAngle(), weaponController.GetFocusSpeed());
            focusSensibility = weaponController.GetFocusSensi();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            firstPersonCamera.Focus(55f, weaponController.GetFocusSpeed());
            focusSensibility = 1f;
        }
    }

    IEnumerator RecoilCo(Vector3 recoil, float duration)
    {
        float counter = 0;
        
        while (counter <= 1f)
        {
            weaponController.RotateUpDown(recoil.y * (float)RecoilFunctionDerivate(counter) * Time.deltaTime * recoilSensitivity);
            counter += Time.deltaTime / duration;
            yield return null;
        }
    }

    double RecoilFunctionDerivate(float t)
    {
        double x4 = t * t * t * t;
        double dividend = -201684.0 * x4 + 84;
        double divisor = (2401.0 * x4 + 3.0) * (2401.0 * x4 + 3.0);
        double result = dividend / divisor;
        return result;
    }
}
