namespace Domain.Enums.Extensions;

public static class OrderStatusExtensions
{
    public static string GetDisplayName(this OrderStatusEnum status)
    {
        return status switch
        {
            OrderStatusEnum.Pending => "Đang chờ xử lý",
            OrderStatusEnum.Confirmed => "Đã xác nhận",
            OrderStatusEnum.Processing => "Đang xử lý",
            OrderStatusEnum.Shipped => "Đã giao vận",
            OrderStatusEnum.Delivered => "Đã giao hàng",
            OrderStatusEnum.Cancelled => "Đã hủy",
            OrderStatusEnum.Returned => "Đã trả lại",
            OrderStatusEnum.Refunded => "Đã hoàn tiền",
            _ => status.ToString()
        };
    }

    public static string GetDescription(this OrderStatusEnum status)
    {
        return status switch
        {
            OrderStatusEnum.Pending => "Đơn hàng đang chờ được xác nhận",
            OrderStatusEnum.Confirmed => "Đơn hàng đã được xác nhận và đang chuẩn bị",
            OrderStatusEnum.Processing => "Đơn hàng đang được xử lý và đóng gói",
            OrderStatusEnum.Shipped => "Đơn hàng đã được giao cho đơn vị vận chuyển",
            OrderStatusEnum.Delivered => "Đơn hàng đã được giao thành công",
            OrderStatusEnum.Cancelled => "Đơn hàng đã bị hủy",
            OrderStatusEnum.Returned => "Đơn hàng đã được trả lại",
            OrderStatusEnum.Refunded => "Đơn hàng đã được hoàn tiền",
            _ => "Trạng thái không xác định"
        };
    }

    public static bool CanTransitionTo(this OrderStatusEnum currentStatus, OrderStatusEnum newStatus)
    {
        return currentStatus switch
        {
            OrderStatusEnum.Pending => newStatus is OrderStatusEnum.Confirmed or OrderStatusEnum.Cancelled,
            OrderStatusEnum.Confirmed => newStatus is OrderStatusEnum.Processing or OrderStatusEnum.Cancelled,
            OrderStatusEnum.Processing => newStatus is OrderStatusEnum.Shipped or OrderStatusEnum.Cancelled,
            OrderStatusEnum.Shipped => newStatus is OrderStatusEnum.Delivered or OrderStatusEnum.Returned,
            OrderStatusEnum.Delivered => newStatus is OrderStatusEnum.Returned,
            OrderStatusEnum.Returned => newStatus is OrderStatusEnum.Refunded,
            OrderStatusEnum.Cancelled => false,
            OrderStatusEnum.Refunded => false,
            _ => false
        };
    }

    public static OrderStatusEnum[] GetValidNextStatuses(this OrderStatusEnum currentStatus)
    {
        return currentStatus switch
        {
            OrderStatusEnum.Pending => [OrderStatusEnum.Confirmed, OrderStatusEnum.Cancelled],
            OrderStatusEnum.Confirmed => [OrderStatusEnum.Processing, OrderStatusEnum.Cancelled],
            OrderStatusEnum.Processing => [OrderStatusEnum.Shipped, OrderStatusEnum.Cancelled],
            OrderStatusEnum.Shipped => [OrderStatusEnum.Delivered, OrderStatusEnum.Returned],
            OrderStatusEnum.Delivered => [OrderStatusEnum.Returned],
            OrderStatusEnum.Returned => [OrderStatusEnum.Refunded],
            OrderStatusEnum.Cancelled => [],
            OrderStatusEnum.Refunded => [],
            _ => []
        };
    }
}