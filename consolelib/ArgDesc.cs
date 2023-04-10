namespace CoolandonRS.consolelib; 

public class ArgDesc {
    public readonly string Template;
    public readonly string Desc;

    /// <summary>
    /// The description for an arg <br/>
    /// IE: -? Shows help menu
    /// </summary>
    /// <param name="template">The template for what the arg might look like. <br/> IE: --val=[int]</param>
    /// <param name="desc">The description for what the value/flag is for <br/> IE: Value to use for calculation</param>
    public ArgDesc(string template, string desc) {
        this.Template = template;
        this.Desc = desc;
    }
}