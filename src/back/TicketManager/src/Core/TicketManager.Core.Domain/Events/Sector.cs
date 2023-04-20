using TicketManager.Core.Domain.Common;

namespace TicketManager.Core.Domain.Events;

public class Sector: 
    IAggregateRoot<Guid>, IOptimisticConcurrent
{
    private readonly List<SeatReservation> seatReservations = new();
    private readonly List<TakenSeat> takenSeats = new();

    public Guid Id { get; private init; }
    public Guid EventId { get; private init; }
    public string Name { get; private set; } = null!;
    public int PriceInSmallestUnit { get; private set; }
    public int NumberOfColumns { get; private set; }
    public int NumberOfRows { get; private set; }
    public int NumberOfSeats => NumberOfRows * NumberOfColumns;
    public IReadOnlyCollection<SeatReservation> SeatReservations => seatReservations.AsReadOnly();
    public IReadOnlyCollection<TakenSeat> TakenSeats => takenSeats.AsReadOnly();
    public DateTime DateModified { get; set; }
    
    public Sector(Guid eventId, string name, int priceInSmallestUnit, int numberOfColumns, int numberOfRows)
    {
        Id = Guid.NewGuid();
        EventId = eventId;
        Name = name;
        PriceInSmallestUnit = priceInSmallestUnit;
        NumberOfColumns = numberOfColumns;
        NumberOfRows = numberOfRows;
    }

    private Sector() { }

    public SeatReservation AddSeatReservation(Guid userId, int numberOfSeats, Guid paymentId)
    {
        return new SeatReservation(Id, userId, numberOfSeats, paymentId);
    }

    public void CloseReservation(Guid paymentId)
    {
        var reservation = seatReservations.First(sr => sr.PaymentId == paymentId);
        reservation.Close();
    }

    public List<TakenSeat> TakeSeats(Guid paymentId)
    {
        var result = new List<TakenSeat>();
        
        var reservation = seatReservations.First(sr => sr.PaymentId == paymentId);
        reservation.Close();

        for (int i = 0; i < NumberOfRows; i++)
        {
            int freeSeats = GetNumberOfFreeSeats(i);
            if (freeSeats >= reservation.ReservedSeatNumber)
            {
                var currentSeats = Enumerable
                    .Range(GetNumberOfTakenSeats(i), reservation.ReservedSeatNumber)
                    .Select(c => new TakenSeat(i, c))
                    .ToList();
                result.AddRange(currentSeats);
                takenSeats.AddRange(currentSeats);
                return result;
            }
        }

        int seatsToReserve = reservation.ReservedSeatNumber;
        for (int i = 0; i < NumberOfRows; i++)
        {
            int freeSeats = GetNumberOfFreeSeats(i);
            if (freeSeats >= seatsToReserve)
            {
                var currentSeats = Enumerable
                    .Range(GetNumberOfTakenSeats(i), seatsToReserve)
                    .Select(c => new TakenSeat(i, c))
                    .ToList();
                takenSeats.AddRange(currentSeats);
                result.AddRange(currentSeats);
                seatsToReserve = 0;
            }
            else
            {
                var currentSeats = Enumerable
                    .Range(GetNumberOfTakenSeats(i), freeSeats)
                    .Select(c => new TakenSeat(i, c))
                    .ToList();
                takenSeats.AddRange(currentSeats);
                result.AddRange(currentSeats);
                seatsToReserve -= freeSeats;
            }
            
            
            if (seatsToReserve == 0)
            {
                break;
            }
        }

        return result;
    }

    private int GetNumberOfFreeSeats(int row)
    {
        return NumberOfColumns - GetNumberOfTakenSeats(row);
    }

    private int GetNumberOfTakenSeats(int row)
    {
        return takenSeats.Count(ts => ts.RowNumber == row);
    }
}
