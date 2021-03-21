using System;

namespace Commons.Results
{
    public class Result<T>
    {
        public Guid ID { get; } = Guid.NewGuid();
        public virtual bool IsSuccess { get; set; } = true;
        public bool IsFailure
        {
            get { return !IsSuccess; }
        }
        public T Data { get; set; }
    }
}
