﻿using UnityEngine;

namespace God_SCRIPT
{
    public class CharacterInput : MonoBehaviour {

        public string keyUp;
        public string keyDown;
        public string keyLeft;
        public string keyRight;


        [Space(1)]
        public string keyJump;
        public string keyElfAttack;

        public string keyBack;

        [SerializeField]
        private Vector2 m_Movement = Vector2.zero;
        [SerializeField]
        private Vector2 m_Camera   = Vector2.zero;

        public float m_MovementForward = 0;
        public float m_MovementRight = 0;

        private float m_CameraForward = 0;
        private float m_CameraRight = 0;
        private bool _enable = true;

        private float m_timeLostX = 0.0f;
        private float m_timeLostY = 0.0f;
        public bool InputEnable
        {
            set
            {
                _enable = value;
                m_MovementForward = 0;
                m_MovementRight = 0;
                m_Movement.x = 0;
                m_Movement.y = 0;
            }
            get
            {
                return _enable;
            }
        }

        public float InputMagic
        {
            get { return Mathf.Sqrt(m_Movement.x * m_Movement.x + m_Movement.y + m_Movement.y); }
        }

        public Vector2 InputVector
        {
            get
            {
                return m_Movement;
            }
        }
        public Vector2 CamerVector
        {
            get
            {
                return m_Camera;
            }
        }
    
        private static CharacterInput instance;
        public static CharacterInput Instance
        {
            get
            {
                return instance;
            }
        }

        private void Awake()
        {
            instance = this;
        }
        // Use this for initialization
        void Start ()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
	
        // Update is called once per frame
        void Update () {

            if(_enable)
            {
                GetKeyBoard();
            }
            GetMouse();
            WalkOrRun();
        }
        private void GetKeyBoard()
        {
            m_MovementRight = UnityEngine.Input.GetAxis("Horizontal");
            m_MovementForward = UnityEngine.Input.GetAxis("Vertical");
            //translate to circle coordinates
            m_Movement.x = m_MovementRight * Mathf.Sqrt(1 - (m_MovementForward * m_MovementForward) / 2.0f);
            m_Movement.y = m_MovementForward * Mathf.Sqrt(1 - (m_MovementRight * m_MovementRight) / 2.0f);
        }
        private void GetMouse()
        {
            m_CameraForward = float.IsNaN(UnityEngine.Input.GetAxis("Mouse Y")) ?0: UnityEngine.Input.GetAxis("Mouse Y");
            m_CameraRight = float.IsNaN(UnityEngine.Input.GetAxis("Mouse X")) ? 0:UnityEngine.Input.GetAxis("Mouse X");
            //translate to circle coordinates
            m_Camera.x = m_CameraRight * Mathf.Sqrt(1 - (m_CameraForward * m_CameraForward) / 2.0f);
            m_Camera.x = float.IsNaN(m_Camera.x) ? 0:m_Camera.x;
            m_Camera.y = m_CameraForward * Mathf.Sqrt(1 - (m_CameraRight * m_CameraRight) / 2.0f);
            m_Camera.y = float.IsNaN(m_Camera.y) ? 0 : m_Camera.y;
        }
        //用于攻击的时候是否进入跑的状态，由于为了更强的效果，所以进入跑这个应该是所有状态都能够进入，思来想去，放在这个里面最为合适
        private void WalkOrRun()
        {
            if (UnityEngine.Input.GetKeyDown(keyUp))
            {
 
                if (Time.time - m_timeLostX <= 0.2f)
                {
                    PlayInfo.Instance._actionInfo = PlayInfo.actionInfo.SprintRun;
                    PlayInfo.Instance._sprintInfo = PlayInfo.sprintInfo.enter;
                    PlayInfo.Instance._adjustVector = new Vector3(0, 360, 0);
                }
                PlayInfo.Instance._characterInfo = PlayInfo.characterInfo.action;
                m_timeLostX = Time.time;
            }
            else if (UnityEngine.Input.GetKeyUp(keyUp))
            {
                PlayInfo.instance._actionInfo = PlayInfo.actionInfo.walk;
                PlayInfo.Instance._characterInfo = PlayInfo.characterInfo.idle;
                PlayInfo.Instance._adjustVector = Vector3.zero;
            }

            if (UnityEngine.Input.GetKeyDown(keyDown))
            {
                if (Time.time - m_timeLostY <= 0.2f)
                {
                    PlayInfo.Instance._actionInfo = PlayInfo.actionInfo.SprintRun;
                    PlayInfo.Instance._sprintInfo = PlayInfo.sprintInfo.enter;
                    PlayInfo.Instance._adjustVector = new Vector3(0,180,0);
                }
                m_timeLostY = Time.time;
                PlayInfo.Instance._characterInfo = PlayInfo.characterInfo.action;
            }
            else if (UnityEngine.Input.GetKeyUp(keyDown))
            {
                PlayInfo.instance._actionInfo = PlayInfo.actionInfo.walk;
                PlayInfo.Instance._characterInfo = PlayInfo.characterInfo.idle;
                PlayInfo.Instance._adjustVector = Vector3.zero;
            }
            if (UnityEngine.Input.GetKeyDown(keyLeft))
            {
                PlayInfo.Instance._adjustVector = new Vector3(0, 90, 0);
                PlayInfo.Instance._characterInfo = PlayInfo.characterInfo.action;
            }
            else if (UnityEngine.Input.GetKeyUp(keyLeft))
            {
                if (PlayInfo.Instance._actionInfo != PlayInfo.actionInfo.Run) PlayInfo.instance._actionInfo = PlayInfo.actionInfo.walk;
                PlayInfo.Instance._characterInfo = PlayInfo.characterInfo.idle;
                PlayInfo.Instance._adjustVector = Vector3.zero;
            }
            if (UnityEngine.Input.GetKeyDown(keyRight))
            {
                PlayInfo.Instance._adjustVector = new Vector3(0, -90, 0);
                PlayInfo.Instance._characterInfo = PlayInfo.characterInfo.action;
            }
            else if (UnityEngine.Input.GetKeyUp(keyRight))
            {
                if (PlayInfo.Instance._actionInfo != PlayInfo.actionInfo.Run) PlayInfo.instance._actionInfo = PlayInfo.actionInfo.walk;
                PlayInfo.Instance._characterInfo = PlayInfo.characterInfo.idle;
                PlayInfo.Instance._adjustVector = Vector3.zero;
            }
        }


        private void Reset()
        {
            keyUp = "w";
            keyDown = "s";
            keyLeft = "a";
            keyRight = "d";

            keyElfAttack = "left shift";
            keyBack = "q";
            keyJump = "space";
        }
    }
}
