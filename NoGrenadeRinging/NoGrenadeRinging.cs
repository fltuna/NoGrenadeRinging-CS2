using System.Runtime.InteropServices;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace NoGrenadeRinging;

public class NoGrenadeRinging: BasePlugin
{
    public override string ModuleName => "NoGrenadeRinging";
    public override string ModuleVersion => "0.1.0";
    public override string ModuleAuthor => "faketuna";
    
    
    // 
    // To find sig. (I have only tried in windows. I'm not sure linux can find it by same way.)
    // 1. String "deafenHE"
    // 2. Check Xrefs to find functions that reference this string
    // 2. Identify a function with 2 parameters
    // 3. Make sig of this function
    // 
    private MemoryFunctionVoid<CBasePlayerPawn, CSound> DeafenHook = new(GameData.GetSignature("GrenadeDeafen"));
    
    
    public override void Load(bool hotReload)
    {
        DeafenHook.Hook(DeafHook, HookMode.Pre);
    }

    public override void Unload(bool hotReload)
    {
        DeafenHook.Unhook(DeafHook, HookMode.Pre);
    }

    private HookResult DeafHook(DynamicHook hook)
    {
        var param2 = hook.GetParam<CSound>(1);

        // If sound volume is lower than 29, then grenade deafen effect will not apply.
        if (param2.Volume <= 29) return HookResult.Continue;
        
        // If sound volume is higher than 30, then set value to 29 to prevent applying the grenade deafen effect.
        Marshal.WriteInt32(param2.Handle + 0xc, 0x1d);
        return HookResult.Continue;
    }
}
