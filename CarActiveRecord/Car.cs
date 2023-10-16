namespace CarActiveRecord;

using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Cars")]
public class Car
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Make { get; set; }

    [Required]
    public string Model { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public decimal Price { get; set; }

    // Constructor
    public Car() { }

    // Constructor for creating a new car
    public Car(string make, string model, int year, decimal price)
    {
        Make = make;
        Model = model;
        Year = year;
        Price = price;
    }

    // Active Record methods
    public static Car Find(int id)
    {
        using var dbContext = new CarDbContext();
        return dbContext.Cars.Find(id);
    }

    public void Save()
    {
        using var dbContext = new CarDbContext();
        if (Id == 0)
        {
            dbContext.Cars.Add(this);
        }
        else
        {
            dbContext.Cars.Update(this);
        }
        dbContext.SaveChanges();
    }

    public void Delete()
    {
        using var dbContext = new CarDbContext();
        dbContext.Cars.Remove(this);
        dbContext.SaveChanges();
    }
}
