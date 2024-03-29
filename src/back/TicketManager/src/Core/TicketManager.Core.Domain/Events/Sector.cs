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

    public void AddSeatReservation(Guid userId, int numberOfSeats, Guid paymentId)
    {
        seatReservations.Add(new(userId, numberOfSeats, paymentId));
    }

    public void CloseReservation(Guid paymentId)
    {
        var reservation = GetReservationByPayment(paymentId);
        reservation.Close();
    }

    public void RemoveAllReservations()
    {
        seatReservations.Clear();
    }

    public void RemoveAllTakenSeats()
    {
        takenSeats.Clear();
    }

    private SeatReservation GetReservationByPayment(Guid paymentId)
    {
        return seatReservations.First(sr => sr.PaymentId == paymentId);
    }

    public bool Update(int priceInSmallestUnit, int numberOfColumns, int numberOfRows)
    {
        var changed = false;
        if (PriceInSmallestUnit != priceInSmallestUnit)
        {
            PriceInSmallestUnit = priceInSmallestUnit;
            changed = true;
        }

        if (NumberOfColumns != numberOfColumns)
        {
            NumberOfColumns = numberOfColumns;
            changed = true;
        }

        if (NumberOfRows != numberOfRows)
        {
            NumberOfRows = numberOfRows;
            changed = true;
        }
        
        return changed;
    }
    
    public List<TakenSeat> TakeSeats(Guid paymentId)
    {
        CloseReservation(paymentId);
        
        var result = new List<TakenSeat>();
        var reservation = GetReservationByPayment(paymentId);

        for (int i = 0; i < NumberOfRows; i++)
        {
            int freeSeats = GetNumberOfNotTakenSeats(i);
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
            int freeSeats = GetNumberOfNotTakenSeats(i);
            int currentSeatsToReserve = Math.Min(freeSeats, seatsToReserve);
            var currentSeats = Enumerable
                .Range(GetNumberOfTakenSeats(i), currentSeatsToReserve)
                .Select(c => new TakenSeat(i, c))
                .ToList();
            takenSeats.AddRange(currentSeats);
            result.AddRange(currentSeats);
            seatsToReserve -= currentSeatsToReserve;

            if (seatsToReserve == 0)
            {
                break;
            }
        }

        return result;
    }

    private int GetNumberOfNotTakenSeats(int row)
    {
        return NumberOfColumns - GetNumberOfTakenSeats(row);
    }

    private int GetNumberOfTakenSeats(int row)
    {
        return takenSeats.Count(ts => ts.RowNumber == row);
    }

    public int GetNumberOfFreeSeats()
    {
        return NumberOfSeats - takenSeats.Count - seatReservations.Where(sr => sr.IsCurrent).Sum(sr => sr.ReservedSeatNumber);
    }
}
