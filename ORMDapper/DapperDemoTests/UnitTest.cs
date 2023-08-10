using DapperDemo.Models;
using FluentAssertions;
namespace DapperDemoTests
{
    public class UnitTest: TestBase
    {
        private List<Product> products = new List<Product>()
        {
            new Product
            {
                Name = "Lenovo",
                Description = "Home Computer, Notebook",
                Weight = 10.5m,
                Length = 20m,
                Height = 5m,
                Width = 8m
            },
            new Product
            {
                Name = "Samsung",
                Description = "TV-set",
                Weight = 17m,
                Length = 500m,
                Height = 200m,
                Width = 80m
            },
            new Product
            {
                Name = "Apple IPhone",
                Description = "Smartphone",
                Weight = 215m,
                Length = 10m,
                Height = 75m,
                Width = 176m
            }
        };
        public static readonly List<Order> orders = new List<Order>
        {
            new Order
            {
                Status = OrderStatus.NotStarted,
                CreatedDate = new DateTime(2022, 1, 7),
                UpdatedDate = new DateTime(2022, 12, 7),
                ProductId = 1
            },
            new Order
            {
                Status = OrderStatus.Done,
                CreatedDate = new DateTime(2023, 1, 17),
                UpdatedDate = new DateTime(2023, 2, 25),
                ProductId = 2
            },
            new Order
            {
                Status = OrderStatus.Arrived,
                CreatedDate = new DateTime(2025, 5, 3),
                UpdatedDate = new DateTime(2025, 8, 7),
                ProductId = 1
            },
            new Order
            {
                Status = OrderStatus.Loading,
                CreatedDate = new DateTime(2024, 1, 17),
                UpdatedDate = new DateTime(2024, 6, 25),
                ProductId = 2
            }
        };
        [Fact]
        public void Should_Add_Product()
        {
            var product = products[0];

            DapperMod.AddProduct(product);

            DapperMod.GetAllProducts().Should().BeEquivalentTo(new List<Product> { product },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Get_All_Products()
        {
            var product1 = products[0];
            var product2 = products[1];

            DapperMod.AddProduct(product1);
            DapperMod.AddProduct(product2);

            DapperMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product1, product2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Update_Product()
        {
            var product1 = products[0];
            var product2 = products[1];
            product2.Id = 1;

            DapperMod.AddProduct(product1);
            DapperMod.UpdateProduct(product2);

            DapperMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product2 });
        }

        [Fact]
        public void Should_Get_Product_By_Id()
        {
            var product1 = products[0];
            var product2 = products[1];

            DapperMod.AddProduct(product1);
            DapperMod.AddProduct(product2);

            DapperMod.GetProductById(1).Should()
                .BeEquivalentTo(product1, config =>
                    config.Excluding(p => p.Id));

            DapperMod.GetProductById(2).Should()
                .BeEquivalentTo(product2, config =>
                    config.Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Delete_Product()
        {
            var product1 = products[0];
            var product2 = products[1];

            DapperMod.AddProduct(product1);
            DapperMod.AddProduct(product2);

            DapperMod.DeleteProduct(1);

            DapperMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Add_Order()
        {
            AddProducts();

            var order = orders[0];

            DapperMod.AddOrder(order);

            DapperMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Get_All_Orders()
        {
            AddProducts();

            var order1 = orders[0];
            var order2 = orders[1];

            DapperMod.AddOrder(order1);
            DapperMod.AddOrder(order2);

            DapperMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order1, order2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Update_Order()
        {
            AddProducts();

            var order1 = orders[0];
            var order2 = orders[1];
            DapperMod.AddOrder(order1);
            DapperMod.AddOrder(order2);

            order1.Id = 1;
            order1.Status = OrderStatus.InProgress;
            order1.UpdatedDate = order1.UpdatedDate.AddDays(10);

            DapperMod.UpdateOrder(order1);

            DapperMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order1, order2 },
                    config => config
                        .Excluding(o => o.Id));
        }

        [Fact]
        public void Should_Get_Order_By_Id()
        {
            AddProducts();

            var order1 = orders[0];
            var order2 = orders[1];
            DapperMod.AddOrder(order1);
            DapperMod.AddOrder(order2);

            DapperMod.GetOrderById(1).Should()
                .BeEquivalentTo(order1, config =>
                    config.Excluding(p => p.Id));

            DapperMod.GetOrderById(2).Should()
                .BeEquivalentTo(order2, config =>
                    config.Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Delete_Order()
        {
            AddProducts();

            var order1 = orders[0];
            var order2 = orders[1];
            DapperMod.AddOrder(order1);
            DapperMod.AddOrder(order2);

            DapperMod.DeleteOrder(1);

            DapperMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Theory]
        [MemberData(nameof(GetFilteredOrdersTestData))]
        public void Should_Get_Filtered_Orders(
            int? year,
            int? month,
            OrderStatus? status,
            int? product,
            List<Order> expected)
        {
            AddProducts();

            DapperMod.AddOrder(orders[0]);
            DapperMod.AddOrder(orders[1]);
            DapperMod.AddOrder(orders[2]);
            DapperMod.AddOrder(orders[3]);

            var result = DapperMod.GetFilteredOrders(
                year: year,
                month: month,
                status: status,
                product: product
                );

            result.Should()
                .BeEquivalentTo(expected, config => config
                            .Excluding(p => p.Id));
        }

        [Theory]
        [MemberData(nameof(DeleteOrdersTestData))]
        public void Should_Delete_Orders(
            int? year,
            int? month,
            OrderStatus? status,
            int? product,
            List<Order> expected)
        {
            AddProducts();

            DapperMod.AddOrder(orders[0]);
            DapperMod.AddOrder(orders[1]);
            DapperMod.AddOrder(orders[2]);
            DapperMod.AddOrder(orders[3]);

            DapperMod.DeleteOrders(
                year: year,
                month: month,
                status: status,
                product: product
            );

            var result = DapperMod.GetAllOrders();

            result.Should()
                .BeEquivalentTo(expected, config => config
                    .Excluding(p => p.Id));
        }

        private void AddProducts()
        {
            DapperMod.AddProduct(products[0]);
            DapperMod.AddProduct(products[1]);
        }

        public static IEnumerable<object[]> GetFilteredOrdersTestData()
        {
            var data = new List<object[]>
        {
            new object[] { null, null, null, null, new List<Order>
            {
                orders[0],
                orders[1],
                orders[2],
                orders[3]
            }},
            new object[] { 2023, null, null, null, new List<Order>
            {
                orders[1]
            }},
            new object[] { null, 5, null, null, new List<Order>
            {
                orders[2]
            }},
            new object[] { null, null, OrderStatus.Loading, null, new List<Order>
            {
                orders[3]
            }},
            new object[] { null, null, null, 2, new List<Order>
            {
                orders[1],
                orders[3]
            }},
            new object[] { 2024, 1, null, null, new List<Order>
            {
                orders[3]
            }},
            new object[] { null, 1, OrderStatus.Loading, null, new List<Order>
            {
                orders[3]
            }},
            new object[] { 2023, 1, OrderStatus.Done, 2, new List<Order>
            {
                orders[1]
            }}
        };

            return data;
        }

        public static IEnumerable<object[]> DeleteOrdersTestData()
        {
            var data = new List<object[]>
        {
            new object[] { null, null, null, null, new List<Order>()},
            new object[] { 2023, null, null, null, new List<Order>
            {
                orders[0],
                orders[2],
                orders[3]
            }},
            new object[] { null, 5, null, null, new List<Order>
            {
                orders[0],
                orders[1],
                orders[3]
            }},
            new object[] { null, null, OrderStatus.Loading, null, new List<Order>
            {
                orders[0],
                orders[1],
                orders[2]
            }},
            new object[] { null, null, null, 2, new List<Order>
            {
                orders[0],
                orders[2]
            }},
            new object[] { 2024, 1, null, null, new List<Order>
            {
                orders[0],
                orders[1],
                orders[2]
            }},
            new object[] { null, 1, OrderStatus.Loading, null, new List<Order>
            {
                orders[0],
                orders[1],
                orders[2]
            }},
            new object[] { 2023, 1, OrderStatus.Done, 2, new List<Order>
            {
                orders[0],
                orders[2],
                orders[3]
            }}
        };

            return data;
        }
    }
}