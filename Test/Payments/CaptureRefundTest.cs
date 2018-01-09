using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using BraintreeHttp;
using Xunit;
using PayPal.Test;
using static PayPal.Test.TestHarness;

namespace PayPal.Payments.Test
{
    [Collection("Payments")]
    public class CaptureRefundTest
    {
        private RefundRequest buildRequestBody()
        {
            return new RefundRequest()
            {
                Amount = new Amount()
                {
                    Total = "10",
                    Currency = "USD"
                },
            };
        }

        [Fact]
        public async void TestCaptureRefundRequest()
        {
            var authCaptureResponse = await AuthorizationCaptureTest.CreateAuthCapture();
            var capture = authCaptureResponse.Result<Capture>();

            CaptureRefundRequest request = new CaptureRefundRequest(capture.Id);
            request.RequestBody(buildRequestBody());

            HttpResponse response = await TestHarness.client().Execute(request);
            Assert.Equal((int) response.StatusCode, 201);
            Assert.NotNull(response.Result<DetailedRefund>());
        }
    }
}