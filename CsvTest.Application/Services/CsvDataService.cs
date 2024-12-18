using CsvHelper;
using CsvTest.Application.Model;
using CsvTest.Infrastructure.Database;
using CsvTest.Infrastructure.Database.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CsvTest.Application.Services
{
    public class CsvDataService
    {
        private readonly CsvTestContext _db;

        public CsvDataService(CsvTestContext db)
        {
            _db = db;
        }

        public async Task<ResponseModel> WriteCsv(IFormFileCollection files)
        {
            if (files.Any())
            {
                List<CsvData> datas = new();

                foreach (var file in files)
                {
                    if (Path.GetExtension(file.FileName).ToLower() != ".csv")
                    {
                        return new ResponseModel
                        {
                            Success = false,
                            Message = $"File {file.FileName} is not a CSV file.",
                        };
                    }

                    using var stream = file.OpenReadStream();
                    using var reader = new StreamReader(stream);
                    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                    var records = csv.GetRecords<CsvRecord>();

                    foreach (var record in records)
                    {
                        TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                        datas.Add(new CsvData
                        {
                            TipAmount = record.TipAmount,
                            FareAmount = record.FareAmount,
                            DOLocationID = record.DOLocationID,
                            PULocationID = record.PULocationID,
                            StoreFwdFlag = record.StoreFwdFlag.Trim() == "N" ? "No" : "Yes",
                            TripDistance = record.TripDistance,
                            TpepPickupDatetime = TimeZoneInfo.ConvertTimeToUtc(record.TpepPickupDatetime, est),
                            TpepDropOffDatetime = TimeZoneInfo.ConvertTimeToUtc(record.TpepDropOffDatetime, est),
                            PassengerCount = record.PassengerCount.HasValue == false ? default : record.PassengerCount.Value,
                        });
                    }
                }

                var duplicateGroups = datas.GroupBy(x => new
                {
                    x.TpepPickupDatetime,
                    x.TpepDropOffDatetime,
                    x.PassengerCount
                })
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Skip(1))
                .ToList();

                using (var writer = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "duplicates.csv")))
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    await csvWriter.WriteRecordsAsync(duplicateGroups);
                }

                datas.RemoveAll(x => duplicateGroups.Any(d => d.TpepPickupDatetime == x.TpepPickupDatetime && d.TpepDropOffDatetime == x.TpepDropOffDatetime && d.PassengerCount == x.PassengerCount));

                _db.AddRange(datas);
                await _db.SaveChangesAsync();

                return new ResponseModel
                {
                    Success = true,
                };
            }
            else
            {
                return new ResponseModel
                {
                    Success = false,
                    Message = "You need to provide at least 1 file.",
                };
            }
        }

        public async Task<ResponseModel<int>> GetHighestTipAmount()
        {
            var topPULocation = await _db.CsvDatas.GroupBy(d => d.PULocationID)
                                                  .Select(g => new
                                                  {
                                                      PULocationID = g.Key,
                                                      AverageTipAmount = g.Average(record => record.TipAmount)
                                                  })
                                                  .OrderByDescending(result => result.AverageTipAmount)
                                                  .FirstOrDefaultAsync();

            if (topPULocation == null)
            {
                return new ResponseModel<int>
                {
                    Success = false,
                    Message = "No data found.",
                };
            }

            return new ResponseModel<int>
            {
                Success = true,
                Result = topPULocation.PULocationID,
            };
        }

        public async Task<ResponseModel<List<CsvRecord>>> GetLongestTripsDistance()
        {
            var trips = _db.CsvDatas.OrderByDescending(d => d.TripDistance).Take(100).Select(t => new CsvRecord
            {
                ID = t.CsvDataID,
                TipAmount = t.TipAmount,
                FareAmount = t.FareAmount,
                PULocationID = t.PULocationID,
                DOLocationID = t.DOLocationID,
                StoreFwdFlag = t.StoreFwdFlag,
                TripDistance = t.TripDistance,
                PassengerCount = t.PassengerCount,
                TpepPickupDatetime = t.TpepPickupDatetime,
                TpepDropOffDatetime = t.TpepDropOffDatetime,
            })
            .ToList();

            return new ResponseModel<List<CsvRecord>>
            {
                Success = true,
                Result = trips
            };
        }

        public async Task<ResponseModel<List<CsvRecord>>> GetLongestTripsByTime()
        {
            var trips = _db.CsvDatas.AsEnumerable().Select(d => new
            {
                Data = d,
                Duration = d.TpepDropOffDatetime - d.TpepPickupDatetime
            })
            .OrderByDescending(t => t.Duration)
            .Take(100)
            .Select(t => new CsvRecord
            {
                ID = t.Data.CsvDataID,
                TipAmount = t.Data.TipAmount,
                FareAmount = t.Data.FareAmount,
                DOLocationID = t.Data.DOLocationID,
                PULocationID = t.Data.PULocationID,
                TripDistance = t.Data.TripDistance,
                StoreFwdFlag = string.IsNullOrWhiteSpace(t.Data.StoreFwdFlag) ? string.Empty : t.Data.StoreFwdFlag,
                PassengerCount = t.Data.PassengerCount,
                TpepPickupDatetime = t.Data.TpepPickupDatetime,
                TpepDropOffDatetime = t.Data.TpepDropOffDatetime,
            })
            .ToList();

            return new ResponseModel<List<CsvRecord>>
            {
                Success = true,
                Result = trips
            };
        }

        public async Task<ResponseModel<List<CsvRecord>>> GetList(int puLocationId)
        {
            var trips = _db.CsvDatas.Where(d => d.PULocationID == puLocationId).Select(t => new CsvRecord
            {
                ID = t.CsvDataID,
                TipAmount = t.TipAmount,
                FareAmount = t.FareAmount,
                DOLocationID = t.DOLocationID,
                PULocationID = t.PULocationID,
                StoreFwdFlag = t.StoreFwdFlag,
                TripDistance = t.TripDistance,
                PassengerCount = t.PassengerCount,
                TpepPickupDatetime = t.TpepPickupDatetime,
                TpepDropOffDatetime = t.TpepDropOffDatetime,
            }).ToList();

            return new ResponseModel<List<CsvRecord>>
            {
                Success = true,
                Result = trips,
            };
        }
    }
}
