// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Game Scripts/Player/Inputs/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""27ec5a78-d67e-444c-b755-e6cd0de11240"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""49c0ac53-1c81-4d7e-a900-7e641b8d672c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rewind"",
                    ""type"": ""Button"",
                    ""id"": ""5c9c737a-5120-4d22-9704-b74d9941213e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""48100679-5d2b-421d-86ac-d1e2264c3e38"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""9994a214-78cc-4e09-b1fe-d0b9a276d27d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HeavyAtkCharge"",
                    ""type"": ""Button"",
                    ""id"": ""a72ad15a-9211-4229-909c-c6c1f6a2c3f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TakeDamageTest"",
                    ""type"": ""Button"",
                    ""id"": ""ae7ac6ba-9ee0-49a8-a725-417e66836976"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HeavyAtkRelease"",
                    ""type"": ""Button"",
                    ""id"": ""ce789060-f5f3-49ec-887e-3dced18e429e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""30af3cc4-f681-41eb-a8e8-5befa9164c57"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0e52bf16-6571-45b1-9ef9-fd6ba493ce4d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Rewind"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""55c1e31b-6435-4965-9ed9-69ae9cb1c36c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Tap(pressPoint=1)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Move"",
                    ""id"": ""d98582d6-e965-4d11-b68b-91f14bea5395"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""689b8c9d-621b-4ebe-a93f-2a9af1bf6c71"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8b8cd9e6-9af6-4dec-8cea-072ac971a621"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1c6f35f6-ac47-496e-9730-3c3f776ae8bb"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""90f19ad4-0815-4c7e-abc4-afe64f54159b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""07ca5504-66f5-49b0-93b0-92b501977523"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc646fe4-7963-4d8b-b24d-b795c579687a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold(duration=0.4)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""HeavyAtkCharge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25da1054-6790-4716-8997-64635595c41f"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""TakeDamageTest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cef6dd00-3ca5-405a-843d-381be4c35f7a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""SlowTap(duration=0.35)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAtkRelease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1d8622a-1ebb-4aed-9dc7-458936bde30f"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""e84cbbcc-29cf-4f15-afcb-4f57c9eed826"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""08fd428f-bee8-440f-b855-4c799671dd72"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ef4c8178-0a19-43e9-8239-1949444259ab"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": []
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Rewind = m_Player.FindAction("Rewind", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Roll = m_Player.FindAction("Roll", throwIfNotFound: true);
        m_Player_HeavyAtkCharge = m_Player.FindAction("HeavyAtkCharge", throwIfNotFound: true);
        m_Player_TakeDamageTest = m_Player.FindAction("TakeDamageTest", throwIfNotFound: true);
        m_Player_HeavyAtkRelease = m_Player.FindAction("HeavyAtkRelease", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Pause = m_Menu.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Rewind;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Roll;
    private readonly InputAction m_Player_HeavyAtkCharge;
    private readonly InputAction m_Player_TakeDamageTest;
    private readonly InputAction m_Player_HeavyAtkRelease;
    private readonly InputAction m_Player_Pause;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Rewind => m_Wrapper.m_Player_Rewind;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Roll => m_Wrapper.m_Player_Roll;
        public InputAction @HeavyAtkCharge => m_Wrapper.m_Player_HeavyAtkCharge;
        public InputAction @TakeDamageTest => m_Wrapper.m_Player_TakeDamageTest;
        public InputAction @HeavyAtkRelease => m_Wrapper.m_Player_HeavyAtkRelease;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Rewind.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRewind;
                @Rewind.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRewind;
                @Rewind.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRewind;
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Roll.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @HeavyAtkCharge.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeavyAtkCharge;
                @HeavyAtkCharge.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeavyAtkCharge;
                @HeavyAtkCharge.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeavyAtkCharge;
                @TakeDamageTest.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTakeDamageTest;
                @TakeDamageTest.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTakeDamageTest;
                @TakeDamageTest.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTakeDamageTest;
                @HeavyAtkRelease.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeavyAtkRelease;
                @HeavyAtkRelease.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeavyAtkRelease;
                @HeavyAtkRelease.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnHeavyAtkRelease;
                @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Rewind.started += instance.OnRewind;
                @Rewind.performed += instance.OnRewind;
                @Rewind.canceled += instance.OnRewind;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @HeavyAtkCharge.started += instance.OnHeavyAtkCharge;
                @HeavyAtkCharge.performed += instance.OnHeavyAtkCharge;
                @HeavyAtkCharge.canceled += instance.OnHeavyAtkCharge;
                @TakeDamageTest.started += instance.OnTakeDamageTest;
                @TakeDamageTest.performed += instance.OnTakeDamageTest;
                @TakeDamageTest.canceled += instance.OnTakeDamageTest;
                @HeavyAtkRelease.started += instance.OnHeavyAtkRelease;
                @HeavyAtkRelease.performed += instance.OnHeavyAtkRelease;
                @HeavyAtkRelease.canceled += instance.OnHeavyAtkRelease;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Pause;
    public struct MenuActions
    {
        private @PlayerInputActions m_Wrapper;
        public MenuActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_Menu_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRewind(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnHeavyAtkCharge(InputAction.CallbackContext context);
        void OnTakeDamageTest(InputAction.CallbackContext context);
        void OnHeavyAtkRelease(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
}
