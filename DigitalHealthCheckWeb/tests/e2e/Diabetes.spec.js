describe("Diabetes page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Diabetes';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        await expect(page).toMatchText("h1", "Diabetes page 1 of 3");

    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Diabetes')).toBeTruthy();
    })

    it("should not go onto next page if only one question answered", async () => {
        // Click #FamilyHistoryDiabetes-2
        page.click('#family-history-2', 'no');
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Diabetes')).toBeTruthy();
 
    }) 

    it("goes to Blood Pressure if both questions answered", async () => {
          // Click #FamilyHistory
        page.click('#family-history', 'yes');
        // Click input[name="Steroids"]
        await page.click('input[name="Steroids"]');
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/BloodSugar')).toBeTruthy;

  })  

  it("should have a back link", async () => {
    // Click text=Back
    await page.click('text=Back');
    expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ3')).toBeTruthy();
  })

})