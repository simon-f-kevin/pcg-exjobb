using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CaveGeneration.Models.Characters
{
    public class StaticEnemy : Enemy
    {

        public StaticEnemy(Texture2D texture, SpriteBatch spiteBatch) : base(texture, spiteBatch, false)
        {
            MaxSpeed = 0;
        }
    }
}
