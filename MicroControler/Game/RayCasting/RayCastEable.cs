using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroControler.Game.RayCasting
{
    public interface RayCastEable
    {
        Vector2f Position { get; }
        float Rotation { get; }
    }
}
