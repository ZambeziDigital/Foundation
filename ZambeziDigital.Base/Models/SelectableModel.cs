namespace ZambeziDigital.Base.Models;

public class SelectableModel<T> 
{
    public bool Selected { get; set; }
    public T Object { get; set; }
}