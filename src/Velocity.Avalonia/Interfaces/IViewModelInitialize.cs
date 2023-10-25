namespace Velocity.Avalonia.Interfaces;

public interface IViewModelInitialize
{
    ValueTask Initialize(object parameter);
}