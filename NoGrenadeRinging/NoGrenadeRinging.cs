using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace NoGrenadeRinging;

public class NoGrenadeRinging: BasePlugin
{
    public override string ModuleName => "NoGreanadeRinging";
    public override string ModuleVersion => "0.0.1";
    public override string ModuleAuthor => "faketuna";
    
    
    // 
    // To find sig. (I have only tried in windows. I'm not sure linux can find it by same way.)
    // 1. String "deafenHE"
    // 2. Check Xrefs to find functions that reference this string
    // 2. Identify a function with 2 parameters
    // 3. Make sig of this function
    // 
    private MemoryFunctionVoid<IntPtr, long> DeafenHook = new(GameData.GetSignature("GrenadeDeafen"));
    
    
    public override void Load(bool hotReload)
    {
        DeafenHook.Hook(DeafHook, HookMode.Pre);;
    }

    public override void Unload(bool hotReload)
    {
        DeafenHook.Unhook(DeafHook, HookMode.Pre);
    }

    private HookResult DeafHook(DynamicHook hook)
    {
        return HookResult.Handled;
    }
}
