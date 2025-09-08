using System;

public interface IHitable
{
    Action OnHit { get; set; }
}
