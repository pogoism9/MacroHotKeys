using PantheonAddonFramework;
using PantheonAddonFramework.Configuration;
using PantheonAddonFramework.Models;
using PantheonAddonFramework.UI;
using System;
using System.Collections.Generic;

namespace MacroHotKeys;

[AddonMetadata("Macro HotKeys", "Xanera", "Allows the use of in-game macros via hotkeys")]
public class MacroHotKeys : Addon
{
    private bool _isActive = true;

    // Create a mapping of keypad numbers to macro names
    // Use int or the appropriate enum type instead of KeyCodes for the dictionary key
    private readonly Dictionary<int, string> _keypadMacroMap;

    public MacroHotKeys()
    {
        // Initialize the dictionary in the constructor
        _keypadMacroMap = new Dictionary<int, string>
        {
            { KeyCodes.Keypad1, "Target Sherder" },
            { KeyCodes.Keypad2, "loc" },
            { KeyCodes.Keypad3, "BUFF" },
            { KeyCodes.Keypad4, "test 1" },
            { KeyCodes.Keypad5, "test 2" },
            { KeyCodes.Keypad6, "test 3" },
            { KeyCodes.Keypad7, "test 4" },
            { KeyCodes.Keypad8, "test 5" }
        };
    }

    public override void OnCreate()
    {
        LifecycleEvents.OnUpdate.Subscribe(OnUpdate);
    }

    private void OnUpdate()
    {
        if (!_isActive)
        {
            return;
        }

        ProcessKeypadMacros();
    }

    // Method to check for key presses and activate macros
    private void ProcessKeypadMacros()
    {
        foreach (var mapping in _keypadMacroMap)
        {
            if (Keyboard.IsKeyDown(mapping.Key))
            {
                var macro = Macros.GetByName(mapping.Value);
                macro?.Activate(false);
            }
        }
    }

    public override void Enable()
    {
        _isActive = true;
    }

    public override void Disable()
    {
        _isActive = false;
    }

    public override IEnumerable<IConfigurationValue> GetConfiguration()
    {
        return Array.Empty<IConfigurationValue>();
    }

    public override void Dispose()
    {
        LifecycleEvents.OnUpdate.Unsubscribe(OnUpdate);
    }
}