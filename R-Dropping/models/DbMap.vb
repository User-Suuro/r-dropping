Public Class Employee
    Public Shared ReadOnly table_name As String = "employee"
    Public Shared ReadOnly id As String = "employee_id"
    Public Shared ReadOnly first_name As String = "first_name"
    Public Shared ReadOnly middle_name As String = "middle_name"
    Public Shared ReadOnly last_name As String = "last_name"
    Public Shared ReadOnly position As String = "position"
    Public Shared ReadOnly created_at As String = "created_at"
End Class

Public Class Buyer
    Public Shared ReadOnly table_name As String = "buyer"
    Public Shared ReadOnly id As String = "buyer_id"
    Public Shared ReadOnly first_name As String = "first_name"
    Public Shared ReadOnly last_name As String = "last_name"
    Public Shared ReadOnly contact_no As String = "contact_no"
    Public Shared ReadOnly address As String = "address"
    Public Shared ReadOnly created_at As String = "created_at"
End Class

Public Class Courier
    Public Shared ReadOnly table_name As String = "courier"
    Public Shared ReadOnly id As String = "courier_id"
    Public Shared ReadOnly first_name As String = "first_name"
    Public Shared ReadOnly last_name As String = "last_name"
    Public Shared ReadOnly vehicle_type As String = "vehicle_type"
    Public Shared ReadOnly vehicle_brand As String = "vehicle_brand"
    Public Shared ReadOnly created_at As String = "created_at"
End Class

Public Class Seller
    Public Shared ReadOnly table_name As String = "seller"
    Public Shared ReadOnly id As String = "seller_id"
    Public Shared ReadOnly seller_name As String = "seller_name"
    Public Shared ReadOnly email As String = "email"
    Public Shared ReadOnly contact_no As String = "contact_no"
    Public Shared ReadOnly platform As String = "platform"
    Public Shared ReadOnly created_at As String = "created_at"
End Class

Public Class Pricing
    Public Shared ReadOnly table_name As String = "pricing"
    Public Shared ReadOnly id As String = "pricing_id"
    Public Shared ReadOnly rate_label As String = "rate_label"
    Public Shared ReadOnly description As String = "description"
    Public Shared ReadOnly base_fee As String = "base_fee"
    Public Shared ReadOnly daily_increment_fee As String = "daily_increment_fee"
End Class

Public Class Storage
    Public Shared ReadOnly table_name As String = "storage_unit"
    Public Shared ReadOnly id As String = "storage_unit_id"
    Public Shared ReadOnly storage_name As String = "storage_name"
    Public Shared ReadOnly storage_type As String = "storage_type"
    Public Shared ReadOnly capacity_limit As String = "capacity_limit"
End Class

' Derived Entities

