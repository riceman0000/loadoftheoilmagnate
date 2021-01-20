using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OilMagnate.StageScene
{
    public interface IDamagable
    {
        void TakeDamage(int amount);

        void TakeDamage(float amount);
    }
}
