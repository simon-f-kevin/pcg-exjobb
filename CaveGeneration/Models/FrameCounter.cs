using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveGeneration.Models
{
    /// <summary>
    /// FPS counter class used mainly for debugging
    /// </summary>
    public class FrameCounter
    {
        public FrameCounter()
        {
        }

        private long TotalFrames { get;  set; }
        private float TotalSeconds { get;  set; }
        private float AverageFramesPerSecond { get;  set; }
        private float CurrentFramesPerSecond { get;  set; }

        private const int MAXIMUM_SAMPLES = 100;

        private Queue<float> _sampleBuffer = new Queue<float>();

        private bool Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
            return true;
        }


        public void Draw(GameTime gameTime, GameWindow window)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Update(deltaTime);

            var fps = string.Format("FPS: {0}", AverageFramesPerSecond);

            window.Title = string.Format("Cave Generation " + fps);

        }

    }
}
