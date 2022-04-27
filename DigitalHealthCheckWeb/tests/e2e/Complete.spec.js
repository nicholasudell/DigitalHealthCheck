describe("Complete page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Complete';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Your communication preferences");
    })

    it("should go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Validation')).toBeTruthy();
    })

    it("allows valid email address to be entered", async () => {
        await page.click('#contact');  
        const checked = await page.isChecked('#contact');
        expect(checked).toBeTruthy();
        await page.fill('#contact-by-email','test@email.address');
        await page.click('button:has-text("Continue")');
        await expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Validation')).toBeTruthy();
        await expect(page).toMatchText("h1", "Validation");
 
    }) 

})