namespace Cottage.API.Exceptions
{
	public abstract class ApplicationsException : Exception
	{
		protected ApplicationsException(string title, string message)
			: base(message) =>
			Title = title;

		public string Title { get; }
	}
}