# PaintStore — Week 3-1 API

本分支保存 ASP.NET Core REST API 作业。旧的控制台项目保留在 `feat/week2-2` 分支。

## 项目结构

- `PaintStore.slnx`：包含两个项目的 Solution。
- `PaintStore.API`：Controller、路由和 Swagger。
- `PaintStore.Models`：产品、订单等模型。

## 运行

需要 .NET 10 SDK。在仓库根目录执行：

```sh
dotnet restore PaintStore.slnx
dotnet build PaintStore.slnx
dotnet run --project PaintStore.API/PaintStore.API.csproj --launch-profile http
```

Swagger：`http://localhost:5253/swagger/index.html`。

## 作业接口

| 方法 | 路径 | 用途 |
|---|---|---|
| GET | `/api/PaintProducts/all` | 获取全部 mock 产品 |
| GET | `/api/Order/all` | 获取全部 mock 订单 |
| POST | `/api/Order/createOrder` | 从 JSON Body 接收订单，返回 201 |
| GET | `/api/Order/FilterOrders?minPrice=100&maxPrice=200` | 返回总价严格大于最小值、严格小于最大值的订单 |

当前使用 mock 数据，没有数据库。订单列表是 Controller 的实例字段，新增订单不会跨请求保留。
