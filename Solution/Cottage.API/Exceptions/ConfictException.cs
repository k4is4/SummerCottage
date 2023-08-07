namespace Cottage.API.Exceptions
{
	public class ConflictException : ApplicationsException
	{
		public ConflictException(string message)
			: base("Conflict Error", message)
		{
		}
	}
}
