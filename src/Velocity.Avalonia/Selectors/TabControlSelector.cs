using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace Velocity.Avalonia.Selectors;

public class TabControlSelector : IDataTemplate
{
    [Content]
    public Dictionary<string, IDataTemplate> Templates { get; } = new();
    
    public Control Build(object param)
    {
        var key = param.ToString();
        if(key is null)
            throw new ArgumentException("Key cannot be null");
        return Templates[key].Build(param);
    }

    public bool Match(object data)
    {
        return data is string key && Templates.ContainsKey(key);
    }
}