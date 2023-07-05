module top(
    output PIN_AA2,
    input AB2,
    output PIN_Y3,
    input AB1,
    output PIN_Y3,
    input AC3,
    output PIN_AA2,
    input AC1,
    output PIN_AA2,
    input PIN_AB3,
    output PIN_BB1,
    input PIN_AB2,
    output PIN_AA2,
    input PIN_AA6,
    output PIN_BB2,
    input PIN_AA5,
    output PIN_AA2,
    input CC2
);
assign PIN_AA2 = AB2;
assign PIN_Y3 = AB1;
assign PIN_Y3 = AC3;
assign PIN_AA2 = AC1;
assign PIN_AA2 = PIN_AB3;
assign PIN_BB1 = PIN_AB2;
assign PIN_AA2 = PIN_AA6;
assign PIN_BB2 = PIN_AA5;
assign PIN_AA2 = CC2;
endmodule
