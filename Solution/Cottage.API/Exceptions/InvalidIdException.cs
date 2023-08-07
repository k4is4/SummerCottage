namespace Cottage.API.Exceptions
{
	public sealed class InvalidIdException : NotFoundException
	{
		public InvalidIdException()
			: base("Id must be a positive number")
		{
		}
	}
}
