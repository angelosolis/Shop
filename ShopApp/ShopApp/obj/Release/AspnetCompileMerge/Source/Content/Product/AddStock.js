function AddStock(id,name)
{
    //
    var htmlBody = "<input type='number' class='form-control' id='qty' value='' required />";
    // Set label
    $('#modalTitle').html("Add Stock (" + name + ")");
    $('#modalBody').html(htmlBody);
    $('#modalBtnOk').html("Save");
    $('#modalBtnCancel').html("Cancel");
    // Set up action
    $('#modalBtnCancel').click(function () {
        $('#myModal').modal('hide');
    });
    $('#modalBtnOk').click(function () {
        var qty = $('#qty').val();
        if (qty == "") {
            $('#qty').css("border", "1px solid red");
            return;
        }
        // reset
        $('#qty').css("border", "1px solid #ced4da");
        // Show prompt
        Swal.fire({
            title: "Are you sure?",
            icon: "question",
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            showCancelButton: true,
            confirmButtonText: "Save",
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                // save here
                $.post("/Shop/ProductStockAdd",
                  {
                      id: id,
                      qty: qty
                  },
                  function (data, status) {
                      if (data.code == 0) {
                          Swal.fire(data.message, "", "success").then((result) => {
                              if (result.isConfirmed) {
                                  window.location = "/Shop/Product";
                              }
                          });
                          $('#myModal').modal('hide');
                      }
                      else {
                          Swal.fire(data.message, "", "danger");
                      }
                  });
            }
        });

    });

    $('#myModal').modal('show');
}