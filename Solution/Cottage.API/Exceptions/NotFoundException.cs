namespace Cottage.API.Exceptions
{
	public abstract class NotFoundException : ApplicationsException
	{
		protected NotFoundException(string message)
			: base("Not Found", message)
		{
		}
	}
}
