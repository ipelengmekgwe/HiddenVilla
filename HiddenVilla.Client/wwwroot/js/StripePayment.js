redirectToCheckout = function (sessionId) {
    var stripe = Stripe('pk_test_51JAG2qFNDcQJumsiZxdOSHnxvlpvTTAOrpX0gfupACAyBsI9BxQ8v6OELuatoYjcp7IOMxR8aKMAV9bFg3x8wy8c00FF76E9aS');
    stripe.redirectToCheckout({
        sessionId: sessionId
    });
};