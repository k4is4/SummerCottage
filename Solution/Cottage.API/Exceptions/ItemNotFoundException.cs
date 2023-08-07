namespace Cottage.API.Exceptions
{
	public sealed class ItemNotFoundException : NotFoundException
	{
		public ItemNotFoundException(int id)
			: base($"Item with the identifier {id} was not found.")
		{
		}
	}
}
