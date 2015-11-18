using System.Collections.Generic;

namespace BoardGame.Domain.Helpers
{
    public class CircularQueue<T> : List<T>
    {
        private int currentPosition;

        public void SetNextItem()
        {
            if (++currentPosition >= Count) currentPosition = 0;
        }

        public T GetCurrentItem => this[currentPosition];
    }
}
