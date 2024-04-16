$("#btnCreate").click(function () {
        //
        var htmlBody = "<input type='text' class='form-control' id='brandname' value='' required />";
        // Set label
        $('#modalTitle').html("Create New Brand Name");
        $('#modalBody').html(htmlBody);
        $('#modalBtnOk').html("Save");
        $('#modalBtnCancel').html("Cancel");
        // Set up action
        $('#modalBtnCancel').click(function () {
            $('#myModal').modal('hide');
        });
        $('#modalBtnOk').click(function () {
            var brandName = $('#brandname').val();
            if (brandName == "") {
                $('#brandname').css("border", "1px solid red");
                return;
            }
            // reset
            $('#brandname').css("border", "1px solid #ced4da");
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
                    $.post("/Shop/BrandCreate",
                      {
                          brandId: 0,
                          brandName: brandName
                      },
                      function (data, status) {
                          if (data.code == 0) {
                              Swal.fire(data.message, "", "success").then((result) => {
                                  if (result.isConfirmed)
                                  {
                                      window.location = "/Shop/Brand";
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
    });
function onEdit(id)
{
    $.get("/Shop/BrandGetById/"+id, function(data, status){
        var brand = data;
        //
        var htmlBody = "<input type='text' class='form-control' id='brandname' value='"+brand.brandName+"' required />";
        // Set label
        $('#modalTitle').html("Update Brand Name("+brand.brandName+")");
        $('#modalBody').html(htmlBody);
        $('#modalBtnOk').html("Update");
        $('#modalBtnCancel').html("Cancel");
        // Set up action
        $('#modalBtnCancel').click(function () {
            $('#myModal').modal('hide');
        });
        $('#modalBtnOk').click(function () {
            var brandName = $('#brandname').val();
            if (brandName == "") {
                $('#brandname').css("border", "1px solid red");
                return;
            }
            // reset
            $('#brandname').css("border", "1px solid #ced4da");
            // assign new brand name
            brand.brandName = brandName;
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
                    $.post("/Shop/BrandUpdate",
                      brand,
                      function (data, status) {
                          if (data.code == 0) {
                              Swal.fire(data.message, "", "success").then((result) => {
                                  if (result.isConfirmed)
                                  {
                                      window.location = "/Shop/Brand";
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
    });
}
function onDelete(id)
{
    Swal.fire({
        title: "Are you sure?",
        icon: "question",
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        showCancelButton: true,
        confirmButtonText: "OK",
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            // save here
            $.post("/Shop/BrandDelete",
              {
                  id: id
              },
              function (data, status) {
                  if (data.code == 0) {
                      Swal.fire(data.message, "", "success").then((result) => {
                          if (result.isConfirmed)
                          {
                              window.location = "/Shop/Brand";
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
}