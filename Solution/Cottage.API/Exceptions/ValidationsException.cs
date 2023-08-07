namespace Cottage.API.Exceptions
{
	public class ValidationsException: ApplicationsException
	{
		public ValidationsException(string message)
			: base("Validation Error", message)
		{
		}
	}
}
